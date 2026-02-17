using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphQLGenerator
{
    public class GenerateRepository 
    {
        private const string BaseNamespace = "ECCSA.BackOffice.Server.Model";
        private readonly string _templatePath;
        private readonly Model _model;
        private readonly Type _context;
        private readonly string _persistencePath;
        private readonly string _graphQLPath;


        private const string Schema = "$$Schema$$";
        private const string Model = "$$Model$$";
        private const string Models = "$$Models$$";
        private const string Include = "_$$Include$$";
        private const string Repository = "$$Repository$$";

        private const string PK = "$$PK$$";

        private const string Dto = "$$dto$$";
        private const string CreateMap = "$$CreateMapp$$";
        private const string UpdateMap = "$$UpdateMapp$$";
        private readonly bool _generateRepositoryFile;
        private readonly bool _generateDtoFile;
        private readonly bool _generateQueryFile;
        private readonly bool _generateMutationFile;
        private readonly bool _generateSubscriptionFile;

        public GenerateRepository(
            Model model 
            , Type contextType
            , string templatePath
            , string persistencePath
            , string graphQLPath
            , bool generateRepositoryFile
            , bool generateDtoFile
            , bool generateQueryFile
            , bool generateMutationFile
            , bool generateSubscriptionFile
            )
        {
            _templatePath = templatePath;
            _persistencePath = persistencePath;
            _graphQLPath = graphQLPath;
            _model = model;
            _context = contextType;
            _generateDtoFile = generateDtoFile;
            _generateMutationFile = generateMutationFile;
            _generateQueryFile = generateQueryFile;
            _generateRepositoryFile = generateRepositoryFile;
            _generateSubscriptionFile = generateSubscriptionFile;
        }

        public void Generate(ref StringBuilder diBuilder, ref StringBuilder conBuilder, ref StringBuilder varBuilder, ref StringBuilder parBuilder )
        {

            string repository = File.ReadAllText(Path.Combine(_templatePath, "Repository.txt"));

            var pluralName = GetPluralName();
            StringBuilder sbCreateMap = new();
            StringBuilder sbUpdateMap = new();
            StringBuilder sbInclude = new();
            var pkType = GuidBasedPK ? "Guid" : "int";
            GenerateDTO(ref sbCreateMap, ref sbUpdateMap, ref sbInclude);
           repository= repository
                        .Replace(Schema, _model.Namespace)
                        .Replace(Model, _model.Name)
                        .Replace(PK, pkType)
                        .Replace(Models, pluralName)
                        .Replace(Include, sbInclude.ToString())
                        .Replace(CreateMap, sbCreateMap.ToString())
                        .Replace(UpdateMap, sbUpdateMap.ToString())
                        ;
            var repPath = Path.Combine(_persistencePath, "Repositories", $"{_model.Namespace}");
            if (!Directory.Exists(repPath))
            {
                Directory.CreateDirectory(repPath);
            }
            var fileName = Path.Combine(repPath, $"{_model.Name}Repository.cs");
            if(_generateRepositoryFile)
                File.WriteAllText(fileName, repository);
            GenerateGraph(ref conBuilder, ref varBuilder, ref parBuilder);
            diBuilder.AppendLine($"services.AddScoped<{_model.Name}Repository>();");
            
        }

        public void GenerateGraph(ref StringBuilder conBuilder, ref StringBuilder varBuilder, ref StringBuilder parBuilder)
        {

            string mutation = File.ReadAllText(Path.Combine(_templatePath, "Mutation.txt"));

            
            var repName = $"{_model.Name.Substring(0, 1).ToLowerInvariant()}{_model.Name[1..]}Repository";

         //   varBuilder.Append("");// Line($"private readonly  {_model.Name}Repository _{repName};");
            conBuilder.AppendLine($"\t\t  _{repName} = {repName};");
            parBuilder.AppendLine($"\t\t , {_model.Name}Repository {repName}");
            var pluralName = GetPluralName();
            mutation = mutation
                         .Replace(Schema, _model.Namespace)
                         .Replace(Model, _model.Name)
                         .Replace(Models, pluralName)
                         .Replace(Repository, $"{repName}")
                         ;
            var mutPath = Path.Combine(_graphQLPath, "Mutations");
            if (!Directory.Exists(mutPath))
            {
                Directory.CreateDirectory(mutPath);
            }
            var fileName = Path.Combine(mutPath, $"Mutation.{_model.Name}.cs");
            if(_generateMutationFile)
                File.WriteAllText(fileName, mutation);

            string query = File.ReadAllText(Path.Combine(_templatePath, "Query.txt"));
            query = query
                        .Replace(Schema, _model.Namespace)
                        .Replace(Model, _model.Name)
                        .Replace(Models, pluralName)
                        .Replace(Repository, repName)
                        ;
            var queryPath = Path.Combine(_graphQLPath, "Queries");
            if (!Directory.Exists(queryPath))
            {
                Directory.CreateDirectory(queryPath);
            }
            fileName = Path.Combine(queryPath, $"Query.{_model.Name}.cs");
            if(_generateQueryFile)
                File.WriteAllText(fileName, query);
        }

        private void GenerateDTO(ref StringBuilder sbCreateMap, ref StringBuilder sbUpdateMap, ref StringBuilder sbInclude )
        {
            StringBuilder sbCreate = new();
            StringBuilder sbUpdate = new();

            string dtoTemplate = File.ReadAllText(Path.Combine(_templatePath, "DTO.txt"));

            sbCreate.AppendLine($"public record Create{_model.Name}Input (");
            sbUpdate.AppendLine($"public record Update{_model.Name}Input (");

            var pInfos = _model.Type.GetProperties();
            var first = true;
            var firstCreate = true;
            var Info = new List<string> { "CreatedBy", "CreateStamp", "UpdatedBy", "UpdateStamp" };
            var createInfo = new List<string>{ "CreatedBy", "CreateStamp"};
            foreach (var pInfo in pInfos)
            {
                var name = pInfo.PropertyType.Name;
                var isEnumType = pInfo.PropertyType.BaseType != null && pInfo.PropertyType.BaseType.Name.Equals("Enum", StringComparison.OrdinalIgnoreCase);
                if (name.StartsWith("Int32"))
                    name = "int";
                var ns = pInfo.PropertyType.Namespace.ToLowerInvariant();
                if ((ns.StartsWith(BaseNamespace.ToLowerInvariant()) && !isEnumType) || ns.Contains("generic"))
                {
                     sbInclude.AppendLine($".Include(x => x.{pInfo.Name})");
                    continue;
                }
                if (!ns.StartsWith("system") && !isEnumType) continue;

                sbCreate.Append('\t');
                sbUpdate.Append('\t');
                sbCreateMap.Append("\t\t\t");
                sbUpdateMap.Append("\t\t\t");
                if (!first)
                {
                    if (!Info.Contains(pInfo.Name)) 
                        sbUpdate.Append(", ");

                    sbCreateMap.Append(", ");
                    
                }
                first = false;
                if (!Info.Contains(pInfo.Name))
                    sbUpdate.AppendLine($"{name} {pInfo.Name}");
                if (pInfo.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    if( name.ToLowerInvariant().StartsWith("guid") )
                        sbCreateMap.AppendLine($"Id = Guid.NewGuid()");
                    continue;
                }
                
                if (!Info.Contains(pInfo.Name))
                {
                        if (!firstCreate)
                        {
                            sbCreate.Append(", ");
                        }
                        sbCreate.AppendLine($"{name} {pInfo.Name}");
                    firstCreate = false;
                }
                if (!Info.Contains(pInfo.Name) )
                {
                    sbCreateMap.AppendLine($"{pInfo.Name} = input.PayLoad.{pInfo.Name}");
                    sbUpdateMap.AppendLine($"tmp.{pInfo.Name} = data.PayLoad.{pInfo.Name};");
                }else
                {
                    if (pInfo.Name.EndsWith("By"))
                    {
                        sbCreateMap.AppendLine($"{pInfo.Name} = input.UserId");
                        if(!pInfo.Name.StartsWith("Creat"))
                            sbUpdateMap.AppendLine($"tmp.{pInfo.Name} = data.UserId;");
                    }else
                    {
                        sbCreateMap.AppendLine($"{pInfo.Name} = DateTime.UtcNow");
                        if (!pInfo.Name.StartsWith("Creat"))
                            sbUpdateMap.AppendLine($"tmp.{pInfo.Name} = DateTime.UtcNow;");
                    }
                }
            }
            sbCreate.AppendLine(");");
            sbCreate.AppendLine();
            sbCreate.AppendLine();
            sbUpdate.AppendLine(");");
            sbUpdate.AppendLine();
            sbUpdate.AppendLine();


            dtoTemplate = dtoTemplate
                        .Replace(Schema, _model.Namespace)
                        .Replace(Model, _model.Name)
                        .Replace(Dto, sbCreate.ToString() + sbUpdate.ToString());
            var dtoPath = Path.Combine(_persistencePath, "DTO", $"{_model.Namespace}");
            if (!Directory.Exists(dtoPath))
            {
                Directory.CreateDirectory(dtoPath);
            }
            var fileName = Path.Combine(dtoPath, $"{_model.Name}Dto.cs");
            if(_generateDtoFile)
                File.WriteAllText(fileName, dtoTemplate.ToString());

        }


       
        private bool GuidBasedPK
        {
            get
            {
                var pInfo = _model.Type.GetProperty("Id");
                var ns = pInfo.PropertyType.Name.ToLowerInvariant();
                if (ns.StartsWith("guid"))
                {
                    return true;
                }
                return false;
            }
        }
        
        private string GetPluralName()
        {
            var properties = _context.GetProperties();
            foreach(var pInfo in properties)
            {
                var arguments = pInfo.GetGetMethod().ReturnType.GenericTypeArguments;
                if (arguments == null || arguments.Length == 0) continue;
                var fullName = arguments[0].FullName;
                if(_model.Type.FullName == fullName)
                {
                    return pInfo.Name;
                }
            }
            return string.Empty;
        }
    }
}
