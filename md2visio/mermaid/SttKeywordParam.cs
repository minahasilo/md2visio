using md2visio.mermaid.@internal;
using Microsoft.Office.Interop.Visio;
using System.Text;

namespace md2visio.mermaid
{
    internal class SttKeywordParam : SynState
    {
        public override SynState NextState()
        {
            if(!Ctx.WithinKeyword()) throw new SynException("syntax error", Ctx);
            if (!HasParam(Ctx)) return SlideSpaces().Forward<SttChar>();

            SlideSpaces();
            SaveParams();
            return SlideSpaces().Forward<SttChar>();
        }

        public static bool HasParam(SynContext ctx)
        {
            bool match = ctx.Test(@"^(?<param>[^\n]+?)(?=\n|;)");
            return match && ctx.TestGroups["param"].Value.Trim().Length > 0;
        }

        void SaveParams()
        {
            var text = new StringBuilder();
            while (true)
            {
                string? next = Peek();
                if(next == null || next == "\n" || next == ";") {
                    AddText(text);
                    break;
                }

                if(SttQuoted.QuotedSequence(Ctx))
                {
                    AddText(text).AddPart("\"", Ctx.TestGroups["quote"].Value);
                    Slide(Ctx.TestGroups["0"].Length);
                }
                else if(next == "[")
                {
                    if (!SttPaired.IsPaired("[", Ctx)) throw new SynException("expect ']'", Ctx);
                    string mid = Ctx.TestGroups["mid"].Value;
                    AddText(text).AddPart("[", mid);
                    Slide(Ctx.TestGroups["0"].Length);
                }
                else if(IsKeyValues(Ctx))
                {
                    AddText(text).AddPart(":", Ctx.TestGroups["kvs"].Value);
                    Slide(Ctx.TestGroups["0"].Length);
                }
                else
                {
                    text.Append(next);
                    Slide();
                }                
            }
            
            Save(partList.MakeWhole());
        }

        SttKeywordParam AddText(StringBuilder text)
        {
            if (text.Length > 0)
            {
                string t = text.ToString();
                AddPart("T", t.Trim());
                text.Clear();
            }
            return this;
        }

        public static bool IsKeyValues(SynContext ctx)
        {
            // e.g. "classDef className fill : #f9f, stroke:#333, stroke-width : 4px ;"
            return ctx.Test(@"^(?<kvs>([^, :\n]+([ \t\v\f]*:[ \t\v\f]*)[^,;\n]+([ \t\v\f]*,[ \t\v\f]*)?)+(?=[;\n]))");
        }
    }
}
