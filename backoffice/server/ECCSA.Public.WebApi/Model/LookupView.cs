using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi.Model
{
    public class LookupView
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public String Name { get; set; }

        public Guid ParentId { get; set; }
    }
    public class TreeView
    {
        public Guid Id { get; set; }
        public String Title { get; set; }

        public List<TreeView> Subs { get; set; }

        public static TreeView BuildTree(List<Association> lst, Guid rootId)
        {
            var r = lst.First(l => l.Id == rootId);
            var root = new TreeView
            {
                Id = rootId,
                Title = r.Name,
                Subs = new List<TreeView>()
            };
            var children = lst.Where(l => l.ParentId == root.Id).ToList();

            BuildTree(children, lst, ref root);

            return root;
        }
        private static void BuildTree(List<Association> childLst, List<Association> allLst, ref TreeView parent)
        {
            var parentId = parent.Id;

            foreach (var c in childLst)
            {

                var childTv = new TreeView
                {
                    Id = c.Id,
                    Title = c.Name
                };
                var children = allLst.Where(l => l.ParentId == parentId).ToList();
                if (children.Count > 0)
                {
                    childTv.Subs = new List<TreeView>();
                    BuildTree(children, allLst, ref childTv);
                }
                parent.Subs.Add(childTv);

            }
        }

        public string ToString()
        {
            var sb = new StringBuilder();
            String json = "[{";
            json += $"id: '{Id}',";
            json += $"title: '{Title}',";
            json += "subs: [";
            //sb.AppendLine("[{");
            //sb.AppendLine($"id: {id},");
            //sb.AppendLine($"title: '{title}',");
            //sb.AppendLine($"subs: [");
            bool firstCall = true;
            foreach (var sub in Subs)
            {
                BuildChild(ref json, sub, firstCall);
                firstCall = false;
            }
            json += "subs: ]}]";
            //sb.AppendLine("]");
            //sb.AppendLine("}]");
            //return sb.ToString();

            return json;

        }

        private void BuildChild(ref String sb, TreeView tv, bool firstCall)
        {
            if (!firstCall)
            {
                sb += " , ";
            }
            sb += "{";
            sb += $"id: {tv.Id},";
            sb += $"title: '{tv.Title}'";
            if (tv.Subs != null && tv.Subs.Count > 0)
            {
                sb += $"subs: [";
                firstCall = true;
                foreach (var sub in tv.Subs)
                {
                    BuildChild(ref sb, sub, firstCall);
                    firstCall = false;
                }
                sb += "}";
                sb += "]";
            }
            else
                sb += "}";

        }
        private void BuildChild(StringBuilder sb, TreeView tv, bool firstCall)
        {
            if (!firstCall)
            {
                sb.Append(" , ");
            }
            sb.AppendLine("{");
            sb.AppendLine($"id: {tv.Id},");
            sb.AppendLine($"title: '{tv.Title}'");
            if (tv.Subs != null && tv.Subs.Count > 0)
            {
                sb.AppendLine($"subs: [");
                firstCall = true;
                foreach (var sub in tv.Subs)
                {
                    BuildChild(sb, sub, firstCall);
                    firstCall = false;
                }
                sb.AppendLine("}");
                sb.AppendLine("]");
            }
            else
                sb.AppendLine("}");

        }
    }
}
