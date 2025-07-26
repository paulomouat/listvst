const Formats = {
    au: "AU",
    vst: "VST",
    vst3: "VST3"
};

const Types = {
    als: "Ableton Live",
    song: "Studio One Song",
    project: "Studio One Mastering Project"
}

let selectedFormats = [];
let selectedTypes = [];

function load() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    Object.keys(Formats).forEach(key => {
        if (urlParams.has(key)) {
            let value = urlParams.get(key);
            if (value === "1") {
                selectedFormats.push(Formats[key]);
            }
        }
    });
    
    if (selectedFormats.length === 0) {
        selectedFormats.push(...Object.values(Formats));
    }
    
    Object.keys(Types).forEach(key => {
        if (urlParams.has(key)) {
            let value = urlParams.get(key);
            if (value === "1") {
                selectedTypes.push(Types[key]);
            }
        }
    });

    if (selectedTypes.length === 0) {
        selectedTypes.push(...Object.values(Types));
    }
    
    updateFormatCheckboxes();
    updateTypeCheckboxes();
    update();
}

function updateFormatCheckboxes() {
    let formatCheckboxes = document.querySelectorAll("#selected-formats input");
    formatCheckboxes.forEach(checkbox => {
        let format = checkbox.id.replace("format-", "");
        checkbox.checked = selectedFormats.includes(Formats[format]);
    });
}

function updateTypeCheckboxes() {
    let typeCheckboxes = document.querySelectorAll("#selected-types input");
    typeCheckboxes.forEach(checkbox => {
        let type = checkbox.id.replace("type-", "");
        checkbox.checked = selectedTypes.includes(Types[type]);
    });
}

function itemHasPluginType(item) {
    let pluginTypeElements = item.querySelectorAll("span.plugintype");
    for (let ptIdx = 0; ptIdx < pluginTypeElements.length; ptIdx++) {
        let pt = pluginTypeElements[ptIdx].innerHTML;
        if (selectedFormats.includes(pt)) {
            return true;
        }
    }
    return false;
}

function itemHasProjectType(item) {
    let projectTypeElements = item.querySelectorAll("span.projecttype");
    for (let ptIdx = 0; ptIdx < projectTypeElements.length; ptIdx++) {
        let pt = projectTypeElements[ptIdx].innerHTML;
        if (selectedTypes.includes(pt)) {
            return true;
        }
    }
    return false;
}

function itemContainerHasPluginType(itemContainer) {
    let items = itemContainer.querySelectorAll("div.item");
    for (let itemIdx = 0; itemIdx < items.length; itemIdx++) {
        let currentItem = items[itemIdx];
        let contains = itemHasPluginType(currentItem);
        if (contains) {
            return true;
        }
    }
    return false;
}

function itemContainerHasProjectType(itemContainer) {
    let items = itemContainer.querySelectorAll("div.item");
    for (let itemIdx = 0; itemIdx < items.length; itemIdx++) {
        let currentItem = items[itemIdx];
        let contains = itemHasProjectType(currentItem);
        if (contains) {
            return true;
        }
    }
    return false;
}

function updateListingByPathIndex() {
    let listingByPath = document.querySelector("#listing-by-path-index");
    let itemContainers = listingByPath.getElementsByClassName("item-container");
    for (const ic of itemContainers) {
        let visible = itemContainerHasPluginType(ic) && itemContainerHasProjectType(ic);
        ic.style.display = visible ? '': 'none';
    }
}

function updateListingByPathEntries() {
    let listingByPath = document.querySelector("#listing-by-path-entries");
    let entries = listingByPath.getElementsByClassName("entry");
    for (const entry of entries) {
        let visible = false;
        if (itemHasProjectType(entry)) {
            let itemContainers = entry.getElementsByClassName("item-container");
            for (const ic of itemContainers) {
                visible = itemContainerHasPluginType(ic);
                if (visible) {
                    break;
                }
            }
        }
        entry.style.display = visible ? '': 'none';
    }
}

function updateListingByPluginIndex() {
    let listingByPlugin = document.querySelector("#listing-by-plugin-index");
    let itemContainers = listingByPlugin.getElementsByClassName("item-container");
    for (let icIdx = 0; icIdx < itemContainers.length; icIdx++) {
        let icVisible = false;
        let currentIc = itemContainers[icIdx];
        let items = currentIc.getElementsByClassName("item");
        for (let itemIdx = 0; itemIdx < items.length; itemIdx++) {
            let currentItem = items[itemIdx];
            let itemVisible = itemHasPluginType(currentItem) /*&& itemHasProjectType(currentItem)*/;
            if (itemVisible) {
                icVisible = true;
            }
            currentItem.style.display = itemVisible ? '': 'none';
        }
        currentIc.style.display = icVisible ? '': 'none';
    }
}

function updateListingByPluginEntries() {
    let listingByPlugin = document.querySelector("#listing-by-plugin-entries");
    let entries = listingByPlugin.getElementsByClassName("entry");
    for(let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        let currentEntry = entries[entryIdx];
        let itemContainers = currentEntry.getElementsByClassName("item-container");
        let entryVisible = false;
        for (let icIdx = 0; icIdx < itemContainers.length; icIdx++) {
            let currentIc = itemContainers[icIdx];
            let icVisible = itemContainerHasPluginType(currentIc) && itemContainerHasProjectType(currentIc);
            if (icVisible) {
                entryVisible = true;
            }
            currentIc.style.display = icVisible ? '': 'none';
        }
        currentEntry.style.display = entryVisible ? '': 'none';
    }
}

function updatePathTotals() {
    let listingIndex = document.querySelector("#listing-by-path-index");
    let stats = listingIndex.querySelector("#stats-project");
    let listingEntries = document.querySelector("#listing-by-path-entries");
    let entries = listingEntries.getElementsByClassName("entry");
    let count = 0;
    for (let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        if (entries[entryIdx].style.display !== 'none') {
            count++;
        }
    }
    stats.innerHTML = count.toLocaleString();
}

function updatePluginTotals() {
    let listingIndex = document.querySelector("#listing-by-plugin-index");
    let stats = listingIndex.querySelector("#stats-plugin");
    let listingEntries = document.querySelector("#listing-by-plugin-entries");
    let entries = listingEntries.getElementsByClassName("entry");
    let count = 0;
    for (let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        if (entries[entryIdx].style.display !== 'none') {
            count++;
        }
    }
    stats.innerHTML = count.toLocaleString()
}

function update() {
    updateListingByPathIndex();
    updateListingByPathEntries();
    updateListingByPluginIndex();
    updateListingByPluginEntries();
    updatePathTotals();
    updatePluginTotals();
}

function updateSelection() {
    selectedFormats = [];
    let formatCheckboxes = document.querySelectorAll("#selected-formats input");
    formatCheckboxes.forEach(checkbox => {
       if (checkbox.checked) {
           let format = checkbox.id.replace("format-", "");
           selectedFormats.push(Formats[format]);
       } 
    });

    selectedTypes = [];
    let typeCheckboxes = document.querySelectorAll("#selected-types input");
    typeCheckboxes.forEach(checkbox => {
        if (checkbox.checked) {
            let type = checkbox.id.replace("type-", "");
            selectedTypes.push(Types[type]);
        }
    });
    update();
}
