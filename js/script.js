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