let formats = ["AU", "VST", "VST3"];
let selectedFormats = [];

let types = ["Ableton Live", "Studio One Song", "Studio One Project"];
let selectedTypes = [];

function load() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    if (urlParams.has('au')) {
        let au = urlParams.get('au');
        if (au === "1") {
            selectedFormats.push("AU");
        }
    }

    if (urlParams.has('vst')) {
        let vst = urlParams.get('vst');
        if (vst === "1") {
            selectedFormats.push("VST");
        }
    }

    if (urlParams.has('vst3')) {
        let vst3 = urlParams.get('vst3');
        if (vst3 === "1") {
            selectedFormats.push("VST3");
        }
    }

    if (!urlParams.has('au') && !urlParams.has('vst') && !urlParams.has('vst3')) {
        selectedFormats.push("AU");
        selectedFormats.push("VST");
        selectedFormats.push("VST3");
    }
    
    if (urlParams.has('als')) {
        let als = urlParams.get('als');
        if (als === "1") {
            selectedTypes.push("Ableton Live");
        }
    }
    
    if (urlParams.has('song')) {
        let song = urlParams.get('song');
        if (song === "1") {
            selectedTypes.push("Studio One Song");
        }
    }

    if (urlParams.has('project')) {
        let project = urlParams.get('project');
        if (project === "1") {
            selectedTypes.push("Studio One Project");
        }
    }
    
    if (!urlParams.has('als') && !urlParams.has('song') && !urlParams.has('project')) {
        selectedTypes.push("Ableton Live");
        selectedTypes.push("Studio One Song");
        selectedTypes.push("Studio One Project");
    }

    updateFormatCheckboxes();
    updateTypeCheckboxes();
    update();
}

function updateFormatCheckboxes() {
    let checkboxes = document.querySelectorAll("#selected-formats input");
    for (let cbIdx = 0; cbIdx < checkboxes.length; cbIdx++) {
        let currentCb = checkboxes[cbIdx];
        if (currentCb.id === "format-au") {
            currentCb.checked = selectedFormats.includes("AU");
        }
        if (currentCb.id === "format-vst") {
            currentCb.checked = selectedFormats.includes("VST");
        }
        if (currentCb.id === "format-vst3") {
            currentCb.checked = selectedFormats.includes("VST3");
        }
    }
}

function updateTypeCheckboxes() {
    let checkboxes = document.querySelectorAll("#selected-types input");
    for (let cbIdx = 0; cbIdx < checkboxes.length; cbIdx++) {
        let currentCb = checkboxes[cbIdx];
        if (currentCb.id === "type-als") {
            currentCb.checked = selectedTypes.includes("Ableton Live");
        }
        if (currentCb.id === "type-song") {
            currentCb.checked = selectedTypes.includes("Studio One Song");
        }
        if (currentCb.id === "type-project") {
            currentCb.checked = selectedTypes.includes("Studio One Project");
        }
    }
}

function itemHasPluginType(item) {
    let pluginTypeElements = item.querySelectorAll("span.pluginType");
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
    for (let icIdx = 0; icIdx < itemContainers.length; icIdx++) {
        let currentIc = itemContainers[icIdx];
        let visible = itemContainerHasPluginType(currentIc) && itemContainerHasProjectType(currentIc);
        currentIc.style.display = visible ? '': 'none';
        //if (!visible) {
        //  currentIc.style.display = 'none';
        //}            
    }
}

function updateListingByPathEntries() {
    let listingByPath = document.querySelector("#listing-by-path-entries");
    let entries = listingByPath.getElementsByClassName("entry");
    for(let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
        let currentEntry = entries[entryIdx];
        let itemContainers = currentEntry.getElementsByClassName("item-container");
        let visible = false;
        for (let icIdx = 0; icIdx < itemContainers.length; icIdx++) {
            let currentIc = itemContainers[icIdx];
            let icVisible = itemContainerHasPluginType(currentIc) && itemContainerHasProjectType(currentIc);
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
    for (let icIdx = 0; icIdx < itemContainers.length; icIdx++) {
        let icVisible = false;
        let currentIc = itemContainers[icIdx];
        let items = currentIc.getElementsByClassName("item");
        for (let itemIdx = 0; itemIdx < items.length; itemIdx++) {
            let currentItem = items[itemIdx];
            let itemVisible = itemHasPluginType(currentItem) && itemHasProjectType(currentItem);
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
    for (let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
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
    for (let entryIdx = 0; entryIdx < entries.length; entryIdx++) {
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
    let formatCheckboxes = document.querySelectorAll("#selected-formats input");
    for (let cbIdx = 0; cbIdx < formatCheckboxes.length; cbIdx++) {
        let currentCb = formatCheckboxes[cbIdx];
        if (currentCb.id === "format-au" && currentCb.checked) {
            selectedFormats.push("AU");
            //console.log("AU checked");
        }
        if (currentCb.id === "format-vst" && currentCb.checked) {
            selectedFormats.push("VST");
            //console.log("VST checked");
        }
        if (currentCb.id === "format-vst3" && currentCb.checked) {
            selectedFormats.push("VST3");
            //console.log("VST3 checked");
        }
    }

    selectedTypes = [];
    let typeCheckboxes = document.querySelectorAll("#selected-types input");
    for (let cbIdx = 0; cbIdx < typeCheckboxes.length; cbIdx++) {
        let currentCb = typeCheckboxes[cbIdx];
        if (currentCb.id === "type-als" && currentCb.checked) {
            selectedTypes.push("Ableton Live");
            //console.log("AU checked");
        }
        if (currentCb.id === "type-song" && currentCb.checked) {
            selectedTypes.push("Studio One Song");
            //console.log("VST checked");
        }
        if (currentCb.id === "type-project" && currentCb.checked) {
            selectedTypes.push("Studio One Project");
            //console.log("VST3 checked");
        }
    }
    /*for (var idx = 0; idx < selectedFormats.length; idx++) {
        console.log(selectedFormats[idx]);
    }*/
    update();
}
