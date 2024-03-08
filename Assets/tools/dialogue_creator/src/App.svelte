<script>
  import QuestsModal from "./QuestsModal.svelte";
  import dialogue from "./dialogue.json";
  import { createEventDispatcher } from "svelte";

  let npcName = dialogue.npcName;
  let quests = dialogue.quests;
  let selectedDialogue = dialogue.dialogue[0];
  let dialogues = dialogue.dialogue;
  const dispatch = createEventDispatcher();

  function updateNode(updatedNode) {
    selectedDialogue.nodes = selectedDialogue.nodes.map((node) =>
      node.id === updatedNode.id ? updatedNode : node,
    );
  }

  function addNode() {
    const nextNodeId = Math.max(...selectedDialogue.nodes.map((n) => n.id)) + 1;

    const newNode = {
      id: isFinite(nextNodeId) ? nextNodeId : 0,
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
    const newName = prompt("Enter name for dialogue...", "common_");
    const newDialogue = {
      id: newName,
      nodes: [],
    };
    dialogues = [...dialogues, newDialogue];
    selectedDialogue = newDialogue;
  }

  function downloadData() {
    const fileData = JSON.stringify(
      { npcName, quests, dialogue: dialogues },
      null,
      2,
    );
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
        npcName = data.npcName;
        quests = data.quests;
        dialogues = data.dialogue;
        selectedDialogue = dialogues[0] || { nodes: [] };
        dispatch("dataLoaded", data);
      } catch (error) {
        console.error("Error parsing JSON:", error);
      }
    };

    reader.readAsText(file);
  }

  let isModalOpen = false;

  function toggleModal() {
    isModalOpen = !isModalOpen;
  }
</script>

<div class="header">
  <select
    bind:value={selectedDialogue}
    on:change={() =>
      (selectedDialogue = dialogues.find((d) => d.id === selectedDialogue.id))}
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

<div class="npc-container">
  <div class="quest-summary">
    <h3>NPC Name</h3>
    <input type="text" name="npcName" bind:value={npcName} />
  </div>

  <div class="quest-summary">
    <h3>Quests</h3>
    <div class="quest-row">
      <small>{quests.join(", ")}</small>
      <button on:click={toggleModal}>Open Quest Editor</button>
    </div>
  </div>
</div>

<QuestsModal bind:showModal={isModalOpen} bind:quests />

<div class="dialogue-container">
  {#each selectedDialogue.nodes as node}
    <div>
      <div>
        <input type="number" value={node.id} disabled class="node-id" />
        <button on:click={() => deleteNode(node.id)}>Delete Node</button>
      </div>

      <textarea bind:value={node.text} />
      {#each node.options as option, optionIndex}
        <div class="option-row">
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
      <hr />
    </div>
  {/each}
  <button on:click={addNode}>Add Node</button>
</div>

<style>
  .header {
    width: 100%;
    background: #080808;
    padding: 1rem;
    gap: 1rem;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .npc-container,
  .dialogue-container {
    max-width: 890px;
    width: 100%;
    margin: 0 auto;
    padding: 2rem;
    padding-bottom: 0.5rem;
  }

  .option-row {
    display: grid;
    grid-template-columns: auto 110px 42px 150px;
    gap: 0.5rem;
    padding-bottom: 0.5rem;
  }

  .node-id {
    width: 42px;
    margin-right: 0.5rem;
  }

  .quest-summary {
    display: flex;
    flex-direction: column;
    gap: 6px;

    & h3 {
      margin: 0;
      padding: 0;
    }
  }

  .npc-container {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 2rem;
  }

  .quest-row {
    display: flex;
    align-items: center;
    justify-content: space-between;
  }
</style>
