<script>
  import dialogue from "./dialogue.json";
  import { createEventDispatcher } from "svelte";

  let selectedDialogue = dialogue.dialogue[0];
  let dialogues = dialogue.dialogue;
  const dispatch = createEventDispatcher();

  function updateNode(updatedNode) {
    selectedDialogue.nodes = selectedDialogue.nodes.map((node) =>
      node.id === updatedNode.id ? updatedNode : node,
    );
  }

  function addNode() {
    const newNode = {
      id: selectedDialogue.nodes.length,
      text: "",
      options: [],
    };
    selectedDialogue.nodes = [...selectedDialogue.nodes, newNode];
  }

  function deleteNode(id) {
    selectedDialogue.nodes = selectedDialogue.nodes.filter(
      (node) => node.id !== id,
    );
  }

  function addOption(node) {
    const updatedOptions = [
      ...node.options,
      { text: "", _action: "goToNode", value: undefined },
    ];
    updateNode({ ...node, options: updatedOptions });
  }

  function deleteOption(node, index) {
    const updatedOptions = [...node.options];
    updatedOptions.splice(index, 1);
    updateNode({ ...node, options: updatedOptions });
  }

  function handleOptionTextChange(node, index, event) {
    const updatedOptions = [...node.options];
    updatedOptions[index].text = event.target.value;
    updateNode({ ...node, options: updatedOptions });
  }

  function handleOptionActionChange(node, index, event) {
    const updatedOptions = [...node.options];
    updatedOptions[index]._action = event.target.value;
    updateNode({ ...node, options: updatedOptions });
  }

  function handleOptionValueChange(node, index, event) {
    const updatedOptions = [...node.options];
    updatedOptions[index].value = parseInt(event.target.value);
    updateNode({ ...node, options: updatedOptions });
  }

  function deleteDialogue(dialogue) {
    dialogues = dialogues.filter((d) => d.id !== dialogue.id);
    selectedDialogue = dialogues[0] || { nodes: [] };
  }

  function createDialogue() {
    const newName = prompt("Enter name for dialogue...", dialogues.length + 1);
    const newDialogue = {
      id: newName,
      nodes: [],
    };
    dialogues = [...dialogues, newDialogue];
    selectedDialogue = newDialogue;
  }

  function downloadData() {
    const fileData = JSON.stringify({ dialogue: dialogues }, null, 2);
    const blob = new Blob([fileData], { type: "text/json" });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.download = "dialogue.json";
    link.href = url;
    link.click();
    URL.revokeObjectURL(url);
  }

  function loadData() {
    const fileInput = document.createElement("input");
    fileInput.type = "file";
    fileInput.accept = "application/json";
    fileInput.addEventListener("change", handleFileChange);
    fileInput.click();
  }

  function handleFileChange(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = () => {
      try {
        const data = JSON.parse(reader.result);
        dialogues = data.dialogue;
        selectedDialogue = dialogues[0] || { nodes: [] };
        dispatch("dataLoaded", data);
      } catch (error) {
        console.error("Error parsing JSON:", error);
      }
    };

    reader.readAsText(file);
  }
</script>

<div class="container">
  <div class="header">
    <select
      bind:value={selectedDialogue}
      on:change={() =>
        (selectedDialogue = dialogues.find(
          (d) => d.id === selectedDialogue.id,
        ))}
    >
      {#each dialogues as dialogue}
        <option value={dialogue}>{dialogue.id}</option>
      {/each}
    </select>
    <button on:click={() => deleteDialogue(selectedDialogue)}
      >Delete Dialogue</button
    >
    <button on:click={createDialogue}>Create Dialogue</button>
    <button on:click={downloadData}>Download JSON</button>
    <button on:click={loadData}>Load JSON</button>
  </div>

  <div>
    <button on:click={addNode}>Add Node</button>
    {#each selectedDialogue.nodes as node}
      <div>
        <textarea bind:value={node.text} />
        {#each node.options as option, optionIndex}
          <div>
            <input
              type="text"
              value={option.text}
              on:input={(e) => handleOptionTextChange(node, optionIndex, e)}
            />
            <select
              value={option._action}
              on:change={(e) => handleOptionActionChange(node, optionIndex, e)}
            >
              <option value="goToNode">Go to Node</option>
              <option value="startQuest">Start Quest</option>
              <option value="endChat">End Chat</option>
            </select>
            <input
              type="number"
              value={option.value || undefined}
              on:input={(e) => handleOptionValueChange(node, optionIndex, e)}
            />
            <button on:click={() => deleteOption(node, optionIndex)}
              >Delete Option</button
            >
          </div>
        {/each}
        <button on:click={() => addOption(node)}>Add Option</button>
        <button on:click={() => deleteNode(node.id)}>Delete Node</button>
      </div>
    {/each}
  </div>
</div>
