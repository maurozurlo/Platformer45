const fs = require('fs');
const path = require('path');

// Function to traverse the directory and find JSON files
function findJSONFiles(dirPath) {
    const files = fs.readdirSync(dirPath);
    const jsonFiles = [];

    for (const file of files) {
        const filePath = path.join(dirPath, file);
        const stats = fs.statSync(filePath);

        if (stats.isDirectory()) {
            jsonFiles.push(...findJSONFiles(filePath));
        } else if (path.extname(file).toLowerCase() === '.json') {
            jsonFiles.push(filePath);
        }
    }

    return jsonFiles;
}

// Function to process the JSON file and generate CSV rows
function processJSONFile(filePath) {
    const data = JSON.parse(fs.readFileSync(filePath, 'utf8'));
    const fileName = path.basename(filePath, '.json');
    if (fileName === "quests") return parseQuests(data);
    if (fileName === "items") return parseItems(data);

    const rows = [`${fileName}_npcName, ${data.npcName}`];

    for (const dialogueObj of data.dialogue) {
        const dialogueId = dialogueObj.id;
        for (const node of dialogueObj.nodes) {
            const nodeIndex = node.id;
            const text = node.text.replace(/,/g, '\\,'); // Escape commas in text
            rows.push(`${fileName}_dialogue_${dialogueId}_${nodeIndex}_text, ${text}`);

            for (let optionIndex = 0; optionIndex < node.options.length; optionIndex++) {
                const option = node.options[optionIndex].text.replace(/,/g, '\\,'); // Escape commas in option text
                rows.push(`${fileName}_dialogue_${dialogueId}_${nodeIndex}_options_${optionIndex}, ${option}`);
            }
        }
    }

    return rows;
}

function parseQuests(data) {
    const rows = []
    for (const quest of data) {
        rows.push(`quest_${quest.id}_quest_name, ${quest.QuestName}`)
    }
    return rows
}

function parseItems(data) {
    const rows = []
    for (const item of data) {
        rows.push(`item_${item.id}_item_name, ${item.itemName}`)
    }
    return rows
}

// Main function
function main() {
    const folderPath = process.argv[2]; // Folder path as command-line argument

    if (!folderPath) {
        console.error('Please provide a folder path as an argument.');
        return;
    }

    const jsonFiles = findJSONFiles(folderPath);
    const csvRows = [];

    for (const filePath of jsonFiles) {
        csvRows.push(...processJSONFile(filePath));
    }

    const csvContent = csvRows.join('\n');
    const outputFilePath = path.join(process.cwd(), 'output.csv');
    fs.writeFileSync(outputFilePath, csvContent);
    console.log(`CSV file saved as: ${outputFilePath}`);
}

main();