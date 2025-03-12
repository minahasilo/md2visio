# md2visio
 convert Markdown (mermaid / mmd) to Visio (vsdx)

AI-assisted programming simplifies the creation of algorithm flowcharts. Many large language models (LLMs) are capable of generating Markdown (Mermaid) flowcharts. However, Markdown editors lack the functionality to edit these flowcharts as intuitively as specialized drawing tools such as Visio, which allow for actions like moving nodes or reorganizing layouts.

The tool md2visio bridges this gap by converting Markdown (Mermaid/MMD) into Visio (.vsdx) format. This enables users to leverage the capabilities of LLMs for initial chart creation while also benefiting from the advanced editing features of Visio for further customization and refinement of the flowcharts.

Mermaid example:

```bat
journey
    %% journey test
    title My working day
    section Go to work
      Make tea: 5: Me
      Go upstairs: 3: Me
      Do work: 1: Me, Cat
    section Go home
      Go downstairs: 5: Me
      Sit down: 5: Me
    section Go home
    	Make tea: 3: Me
```

Generated Visio graph:

<img src="https://github.com/Megre/md2visio/blob/main/example.png" alt="journey.vssx" style="zoom: 50%;" />

# Supported Mermaid Figure

Development plan:

- [x] graph / flowchart
- [x] journey
- [ ] sequenceDiagram
- [ ] classDiagram
- [ ] stateDiagram
- [ ] stateDiagram-v2
- [ ] erDiagram
- [ ] journey
- [ ] gantt
- [ ] pie
- [ ] quadrantChart
- [ ] requirementDiagram
- [ ] gitGraph
- [ ] C4Context
- [ ] mindmap
- [ ] timeline
- [ ] zenuml
- [ ] sankey-beta
- [ ] xychart-beta
- [ ] block-beta
- [ ] packet-beta
- [ ] kanban
- [ ] architecture-beta
