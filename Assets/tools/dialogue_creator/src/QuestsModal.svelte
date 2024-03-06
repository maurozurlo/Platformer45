<script>
  export let showModal = false;
  export let quests = []; // Initial list of quests
  let newQuestId = ""; // New quest ID to be added

  function addQuest() {
    const parsedId = parseInt(newQuestId, 10);
    if (!isNaN(parsedId) && !quests.includes(parsedId)) {
      quests = [...quests, parsedId];
      newQuestId = "";
    }
  }

  function removeQuest(id) {
    quests = quests.filter((quest) => quest !== id);
  }
</script>

{#if showModal}
  <div class="modal">
    <div class="modal-content">
      <div>
        <div class="modal-header">
          <h2>Quests</h2>
          <button on:click={() => (showModal = false)}>X</button>
        </div>
        <ul class="quest-list">
          {#each quests as quest (quest)}
            <li>
              {quest}
              <button on:click={() => removeQuest(quest)}>Remove</button>
            </li>
          {/each}
        </ul>
      </div>
      <div class="footer">
        <div class="footer-add">
          <input
            type="text"
            bind:value={newQuestId}
            placeholder="New Quest ID"
          />
          <button on:click={addQuest}>Add Quest</button>
        </div>
        <!--<button>Save</button>-->
      </div>
    </div>
  </div>
{/if}

<style>
  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .modal-content {
    background-color: black;
    padding: 20px;
    min-height: 650px;
    border-radius: 4px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-between;
  }

  .modal-header {
    display: flex;
    align-items: center;
    justify-content: space-between;

    & button {
      min-width: 40px;
    }
  }

  .quest-list {
    list-style-type: none;
    padding: 0;
    min-width: 300px;
  }

  .quest-list li {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
  }

  .footer {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .footer-add {
    display: flex;
    gap: 1rem;
  }
</style>
