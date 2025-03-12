namespace md2visio.mermaid.cmn
{
    internal class SynStateMachine
    {
        SynContext context;
        public SynStateMachine(SynContext context) { this.context = context; }

        public void Start()
        {
            SynState state = SttMermaidStart.Run(context);
            Console.Write(context.ToString());
        }
    }
}
