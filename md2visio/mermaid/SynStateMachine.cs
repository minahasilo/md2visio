namespace md2visio.mermaid
{
    internal class SynStateMachine
    {
        SynContext context;
        public SynStateMachine(SynContext context) { this.context = context; }

        public void Start()
        {
            SynState state = SttMermaidStart.Run(context);
            //while (state != SynState.EndOfFile) 
            //    state = state.NextState();

            Console.Write(context.ToString());
        }
    }
}
