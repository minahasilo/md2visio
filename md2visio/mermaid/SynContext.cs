using System.Text;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SynContext
    {
        public static SynContext Empty = new SynContext();
        List<SynState> stateList = new List<SynState>();

        StringBuilder text = new StringBuilder();
        StringBuilder consumed = new StringBuilder();
        StringBuilder cache = new StringBuilder();
        GroupCollection matchedGroup = Regex.Match("", "").Groups;
        public int Line { get; set; } = 1;
        public GroupCollection MatchedGroups { get { return matchedGroup; } }
        public GroupCollection TestGroups { get; set; } = Regex.Match("", "").Groups;
        public List<SynState> StateList { get { return stateList; } }
        public string Text { get { return text.ToString(); } }
        public StringBuilder TextBuilder { get { return text; } }

        SynContext() { }

        public string Cache { get { return cache.ToString(); } }
        public SynContext(string[] lines) { 
            foreach(string line in lines) text.Append(line).Append('\n');
        }

        public string? Peek(int length=1) {
            if(length > text.Length || length < -consumed.Length) return null;
            if (length >= 0) return text.ToString(0, length);
            return consumed.ToString(consumed.Length+length, -length);
        }
        public string? Take(int length = 1) { 
            if(length < 0 || length > text.Length) return null;

            string take = text.ToString(0, length); 
            consumed.Append(take);
            cache.Append(take);
            Skip(length);
            return take;
        }
        public void Skip(int length=1) 
        { 
            text.Remove(0, Math.Min(length, text.Length)); 
        }

        public string? Slide(int length = 1)
        {
            if (length < 0 || length > text.Length) 
                throw new ArgumentOutOfRangeException("length");

            string slide = text.ToString(0, length);
            consumed.Append(slide);
            Skip(length);
            return slide;
        }

        public void Restore(int length)
        {
            if (length <= 0) 
                throw new ArgumentOutOfRangeException("length");

            length = Math.Min(length, consumed.Length);          

            int start = consumed.Length - length;
            text.Insert(0, consumed.ToString(start, length));
            consumed.Remove(start, length);
        }

        public void Restore(string text)
        {
            this.text.Insert(0, text);
        }

        public SynState LastState()
        {
            if(stateList.Count < 1) return SynState.Empty;

            return stateList.Last();
        }

        public SynState LastNonFinishState()
        {
            for(int i=stateList.Count-1; i>=0; --i)
            {
                if (stateList[i] is not SttFinishFlag) return stateList[i];
            }

            return SynState.Empty;
        }

        public bool Expect(string pattern, bool multiline=false)
        {
            Match match = Regex.Match(text.ToString(), $"^(?<tail>{pattern})",
                multiline ? RegexOptions.Multiline : RegexOptions.None); 
            if (match.Success) { 
                consumed.Append(match.Groups[0].Value);
                matchedGroup = match.Groups;
                text.Remove(0, match.Length);
                return true; 
            }
            return false;
        }
        public bool Until(string pattern, bool multiline=true)
        {
            Match match = Regex.Match(text.ToString(0, text.Length), $"^(?<head>.*?)(?<tail>{pattern})", 
                multiline?RegexOptions.Multiline:RegexOptions.None);
            if (match.Success) {
                consumed.Append(match.Groups[0].Value);
                matchedGroup = match.Groups;
                text.Remove(0, match.Index + match.Length);
                return true; 
            }
            return false;
        }

        public bool Test(string pattern)
        {
            Match match = Regex.Match(Text, pattern);
            if(!match.Success) return false;

            TestGroups = match.Groups;
            return true;
        }

        public void AddState(SynState state) { 
            if(stateList.Count > 0)
            {
                if (stateList.Last() is SttFinishFlag &&
                    state is SttFinishFlag) return;
            }
            stateList.Add(state);
        }

        public (bool Success, SynState Container) FindContainerType(string stateNamePattern)
        {
            for (int i = stateList.Count - 1; i >= 0; i--) 
            {
                SynState state = stateList[i];
                string typeName = state.GetType().Name;
                if (Regex.IsMatch(typeName, $"^({stateNamePattern})$")) return (true, state);
            }
            return (false, SynState.Empty);
        }

        public (bool Success, string Fragment) FindContainerFrag(string fragmentPattern)
        {
            for (int i = stateList.Count - 1; i >= 0; i--)
            {
                string frag = stateList.ElementAt(i).Fragment;
                if (Regex.IsMatch(frag, $"^({fragmentPattern})$")) return (true, frag);
            }
            return (false, string.Empty);
        }

        public bool WithinKeyword()
        {
            for (int i = stateList.Count - 1; i >= 0; i--)
            {
                SynState state = stateList.ElementAt(i);
                if (state is SttFinishFlag ) return false;
                if (SttKeyword.IsKeyword(state.Fragment)) return true;
            }
            return false;
        }

        public void ClearCache() { cache.Clear(); }

        public SttIterator SttIterator()
        {
            return new SttIterator(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SynState state in stateList) { 
                sb.Append(state.ToString()).Append('\n');
            }
            return sb.ToString();
        }
    }
}
