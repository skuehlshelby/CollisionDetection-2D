function getSimulationHeight() {
    return Math.floor(document.getElementById("simulationCanvas").getBoundingClientRect().height);
}

function setCanvasHeight(height) {
    document.getElementById("simulationCanvas").querySelector("canvas").height = height;
}

function getSimulationWidth() {
    return Math.floor(document.getElementById("simulationCanvas").getBoundingClientRect().width);
}

function setCanvasWidth(width) {
    document.getElementById("simulationCanvas").querySelector("canvas").width = width;
}

//New

function addResizeListener(objectReference, element) {
    window.addEventListener("resize", () => objectReference.invokeMethodAsync("Resized", getHeight(element), getWidth(element)));
}

function getHeight(element) {
    return Math.floor(element.getBoundingClientRect().height);
}

function getWidth(element) {
    return Math.floor(element.getBoundingClientRect().width);
}

function setSize(element, height, width) {
    element.height = height;
    element.width = width;
}

function setHeight(element, height) {
    element.height = height;
}

function setWidth(element, width) {
    element.width = width;
}

function isOverflownVertically(elementId) {
    var element = document.getElementById(elementId);
    return element.scrollHeight > element.clientHeight;
}

function isOverflownHorizontally(elementId) {
    var element = document.getElementById(elementId);
    return element.scrollWidth > element.clientWidth;
}