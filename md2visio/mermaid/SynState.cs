using md2visio.figure;
using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal abstract class SynState: IEmpty
    {
        public static SynState EndOfFile = new SttEmpty();
        public static SynState Empty = new SttEmpty();
        
        SynContext context = SynContext.Empty;
        protected PartValueList partList = new PartValueList();
        protected GroupCollection MatchedGroups { get { return Ctx.MatchedGroups; } }
        public SynContext Ctx { get { return context; } set { context = value; } }

        public string Fragment { get { return partList.Whole; } set { partList.Whole = value; } }
        public PartValueList PartList { get { return partList; } }
        public string Cache { get { return Ctx.Cache;  } }

        public SynState Clear()
        {
            Ctx.ClearCache();
            return this;
        }

        public string? Peek(int length=1)
        {
            return Ctx.Peek(length);
        }

        public SynState SlideSpaces()
        {
            string? next = Ctx.Peek();
            while (next != null)
            {
                if (next == "\n") break;
                if (!string.IsNullOrWhiteSpace(next)) break;
                Ctx.Slide();
                next = Ctx.Peek();
            }

            return this;
        }

        public SynState Take(int length = 1)
        {
            Ctx.Take(length);
            return this;
        }

        public SynState Slide(int length = 1)
        {
            Ctx.Slide(length); 
            return this;
        }

        protected bool Expect(string pattern, bool multiline=false)
        {
            return Ctx.Expect(pattern, multiline);
        }

        public bool Until(string pattern, bool multiline=true) { 
            return Ctx.Until(pattern, multiline); 
        }


        public SynState Restore(int length = 1)
        {
            Ctx.Restore(length);
            return this;
        }

        public string Sequence(string groupName)
        {
            if (MatchedGroups.Count > 0) return MatchedGroups[groupName].Value;
            return string.Empty;
        }

        public abstract SynState NextState();

        public SynState Forward<T>() where T : SynState, new()
        {
            return Create<T>().NextState();
        }

        public SynState Create<T>() where T : SynState, new()
        {
            SynState clone = new T();
            clone.Ctx = Ctx;
            return clone;
        }

        public SynState Save(string frag)
        {
            partList.Whole = frag;
            Ctx.AddState(this);
            return this;
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        public bool IsEndOfFile() { return this == EndOfFile; }

        public string GetPart(string name)
        {
            return partList.Get(name);
        }

        public string GetPart(int index)
        {
            return partList.Get(index);
        }

        public void AddPart(string name, string value)
        {
            partList.Add(name, value);
        }

        public void AddPart(string value)
        {
            partList.Add(value);
        }

        public override string ToString()
        {
            if (this is SttFinishFlag) return "■";
            return string.Format("{0} \t@ {1}", partList.Whole, GetType().Name);
        }

    }
}
