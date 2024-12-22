let formats = ["AU", "VST", "VST3"];
let selectedFormats = [];

function itemHasPluginType(item) {
    let pluginTypeElements = item.querySelectorAll("span.pluginType");
    for (var ptIdx = 0; ptIdx < pluginTypeElements.length; ptIdx++) {
        let pt = pluginTypeElements[ptIdx].innerHTML;
        if (selectedFormats.includes(pt)) {
            return true;
        }
    }
    return false;
}

function itemContainerHasPluginType(itemContainer) {
    let items = itemContainer.querySelectorAll("div.item");
    for (var itemIdx = 0; itemIdx < items.length; itemIdx++) {
        let currentItem = items[itemIdx];
        let contains = itemHasPluginType(currentItem);
        if (contains) {
            return true;
        }
    }
    return false;
}

function updateListingByPathIndex() {
    let listingByPath = document.querySelector("#listing-by-path-index");
    let itemContainers = listingByPath.getElementsByClassName("item-container");
    for (var icIdx = 0; icIdx < itemContainers.length; icIdx++) {
        let currentIc = itemContainers[icIdx];
        let visible = itemContainerHasPluginType(currentIc);
        currentIc.style.display = visible ? '': 'none';
        //if (!visible) {
        //  currentIc.style.display = 'none';
        //}            
    }
}

function updateListingByPathEntries() {
    let listingByPath = document.querySelector("#listing-by-path-entries");
    let entries = listingByPath.getElementsByClassName("entry");
    for(var entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        let currentEntry = entries[entryIdx];
        let itemContainers = currentEntry.getElementsByClassName("item-container");
        let visible = false;
        for (var icIdx = 0; icIdx < itemContainers.length; icIdx++) {
            let currentIc = itemContainers[icIdx];
            let icVisible = itemContainerHasPluginType(currentIc);
            if (icVisible) {
                visible = true;
                break;
            }
        }
        currentEntry.style.display = visible ? '': 'none';
        //if (!visible) {
        //  currentEntry.style.display = 'none';              
        //}            
    }
}

function updateListingByPluginIndex() {
    let listingByPlugin = document.querySelector("#listing-by-plugin-index");
    let itemContainers = listingByPlugin.getElementsByClassName("item-container");
    for (var icIdx = 0; icIdx < itemContainers.length; icIdx++) {
        let icVisible = false;
        let currentIc = itemContainers[icIdx];
        let items = currentIc.getElementsByClassName("item");
        for (var itemIdx = 0; itemIdx < items.length; itemIdx++) {
            let currentItem = items[itemIdx];
            let itemVisible = itemHasPluginType(currentItem);
            if (itemVisible) {
                icVisible = true;
            }
            currentItem.style.display = itemVisible ? '': 'none';
            //if (itemVisible) {
            //  icVisible = true;
            //} else {
            //  currentItem.style.display = 'none';
            //}
        }
        currentIc.style.display = icVisible ? '': 'none';
        //if (!icVisible) {
        //  currentIc.style.display = 'none';
        //}            
    }
}

function updateListingByPluginEntries() {
    let listingByPlugin = document.querySelector("#listing-by-plugin-entries");
    let entries = listingByPlugin.getElementsByClassName("entry");
    for(var entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        let currentEntry = entries[entryIdx];
        let itemContainers = currentEntry.getElementsByClassName("item-container");
        let entryVisible = false;
        for (var icIdx = 0; icIdx < itemContainers.length; icIdx++) {
            let currentIc = itemContainers[icIdx];
            let icVisible = itemContainerHasPluginType(currentIc);
            if (icVisible) {
                entryVisible = true;
            }
            currentIc.style.display = icVisible ? '': 'none';
            //if (icVisible) {
            //  entryVisible = true;
            //} else {
            //  currentIc.style.display = 'none';
            //}     
        }
        currentEntry.style.display = entryVisible ? '': 'none';
        //if (!entryVisible) {
        //  currentEntry.style.display = 'none';
        //}            
    }
}

function updatePathTotals() {
    let listingIndex = document.querySelector("#listing-by-path-index");
    let stats = listingIndex.querySelector("#stats-project");
    let listingEntries = document.querySelector("#listing-by-path-entries");
    let entries = listingEntries.getElementsByClassName("entry");
    let count = 0;
    for (var entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        if (entries[entryIdx].style.display !== 'none') {
            count++;
        }
    }
    stats.innerHTML = count;
}

function updatePluginTotals() {
    let listingIndex = document.querySelector("#listing-by-plugin-index");
    let stats = listingIndex.querySelector("#stats-plugin");
    let listingEntries = document.querySelector("#listing-by-plugin-entries");
    let entries = listingEntries.getElementsByClassName("entry");
    let count = 0;
    for (var entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        if (entries[entryIdx].style.display !== 'none') {
            count++;
        }
    }
    stats.innerHTML = count;
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
    let checkboxes = document.querySelectorAll("#selected-formats input");
    for (var cbIdx = 0; cbIdx < checkboxes.length; cbIdx++) {
        let currentCb = checkboxes[cbIdx];
        if (currentCb.id === "format-au" && currentCb.checked) {
            selectedFormats.push("AU");
            console.log("AU checked");
        }
        if (currentCb.id === "format-vst" && currentCb.checked) {
            selectedFormats.push("VST");
            console.log("VST checked");
        }
        if (currentCb.id === "format-vst3" && currentCb.checked) {
            selectedFormats.push("VST3");
            console.log("VST3 checked");
        }
    }
    for (var idx = 0; idx < selectedFormats.length; idx++) {
        console.log(selectedFormats[idx]);
    }
    update();
}
