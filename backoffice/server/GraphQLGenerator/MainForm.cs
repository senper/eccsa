using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphQLGenerator
{
    public partial class MainForm : Form
    {
        private HashSet<Namespace> namespaces;
        private List<Model> models;
        private Type context;
        private const string BaseNamespace = "ECCSA.BackOffice.Server.Model.";

        private readonly string PersistencePath = @"C:\Projects\tmp\code\backoffice\server\Persistence";
        private readonly string GraphQLPath = @"C:\Projects\tmp\code\backoffice\server\API\GraphQL";
        private readonly string TemplatePath = @"C:\Projects\eccsa\code\backoffice\server\GraphQLGenerator\Template";
        public MainForm()
        {
            InitializeComponent();
            ReflectNamespace();
        }
        private void ReflectNamespace()
        {
            lbNamespaces.Items.Clear();
            DbContextOptions<EccsaContext> options = new();
            
            context = new EccsaContext(options).GetType();
            var ob = new ECCSA.BackOffice.Server.Model.Membership.Stakeholder();
            
            var assembly = ob.GetType().Assembly;
            var types = assembly.GetTypes();
            namespaces = new HashSet<Namespace>();
            models = new List<Model>();
            foreach(var type in types)
            {
                var shortNamespace = type.Namespace.Replace(BaseNamespace, "");
                var isEnumType = type.BaseType != null && type.BaseType.Name.Equals("Enum", StringComparison.OrdinalIgnoreCase);
                if (isEnumType) continue;
                namespaces.Add(new Namespace(shortNamespace, type.Namespace));
                models.Add(new Model(
                    type.Name,
                    shortNamespace,
                    type
                    ));
            }
            foreach(var ns in namespaces)
            {
                lbNamespaces.Items.Add(ns.ShortName);
            }
        }

        

        private void lbNamespaces_SelectedValueChanged(object sender, EventArgs e)
        {
            lvModels.Items.Clear();
            var shortName = lbNamespaces.SelectedItem.ToString();
            var lst = models.Where(m => m.Namespace == shortName);
            foreach(var mdl in lst)
            {
                var li = new ListViewItem(mdl.Name);
                li.SubItems.Add(mdl.Namespace);
                li.Tag = mdl;
                lvModels.Items.Add(li);
            }
        }

        private void GenerateCode_Click(object sender, EventArgs e)
        {
            StringBuilder diBuilder = new();
            StringBuilder conBuilder = new();

            StringBuilder parBuilder = new();
            StringBuilder varBuilder = new();
            for (var i = 0; i < lvModels.SelectedItems.Count; i++)
            {
                var model = (Model)lvModels.SelectedItems[i].Tag;
                var generator = new GenerateRepository(
                     model
                    , context
                    , TemplatePath
                    , PersistencePath
                    , GraphQLPath
                    , chkRepository.Checked
                    , chkDto.Checked
                    , chkQuery.Checked
                    , chkMutation.Checked
                    , chkSubscription.Checked

                    );
              generator.Generate(ref diBuilder, ref conBuilder, ref varBuilder, ref parBuilder);

            }
            var fileName = Path.Combine(GraphQLPath, "tmp.txt");
            string content = string.Empty;
            if (File.Exists(fileName))
            {
                content = File.ReadAllText(fileName);
            }

            content += diBuilder.ToString() //;
            +"\n\n" + conBuilder.ToString()
            + "\n\n" + parBuilder.ToString()
            + "\n\n" + varBuilder.ToString();
            File.WriteAllText(fileName, content);

            MessageBox.Show("Completed");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }

    public record Namespace(string ShortName, string FullName);
    public record Model(string Name, string Namespace, Type Type);
}
