function registerResizeCallback(objectRef) {
    window.addEventListener("resize", () => invokeResize(objectRef));
}

function invokeResize(objectRef) {
    objectRef.invokeMethodAsync("OnWindowResize", getWindowHeight(), getWindowWidth());
}

function getWidth(element) {
    return Math.floor(element.getBoundingClientRect().width);
}

function getHeight(element) {
    return Math.floor(element.getBoundingClientRect().height);
}

function setDimensions(element, height, width) {
    element.height = height;
    element.width = width;
}

function getWindowHeight() {
    return Math.floor(window.innerHeight);
}

function getWindowWidth() {
    return Math.floor(window.innerWidth);
}