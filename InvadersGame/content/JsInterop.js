// Javascript functions callable from .NET, and functions which can call .NET functions.

var sounds = [];

window.JsFunctions = {
    showPrompt: function (message) {
        return prompt(message, 'Type anything here');
    },
    showAlert: function (message) {
        alert(message);
        return true;
    },
    initialiseSound: function (obj) {
        var index = sounds.findIndex(x => x === obj["id"]);

        if (index === -1) {
            var audio = new Audio(obj["path"]);

            if (obj["loop"]) {
                audio.loop = true;
            }

            sounds.push(audio);
        }

        return true;
    },
    playSound: function (id) {
        const audio = sounds[id];

        audio.currentTime = 0;
        audio.play();

        return true;
    }};

document.onkeydown = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('InvadersGame', 'JsKeyDown', evt.keyCode);

    //Prevent all but F5 and F12
    if (evt.keyCode !== 116 && evt.keyCode !== 123)
        evt.preventDefault();
};

document.onkeyup = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('InvadersGame', 'JsKeyUp', evt.keyCode);

    //Prevent all but F5 and F12
    if (evt.keyCode !== 116 && evt.keyCode !== 123)
        evt.preventDefault();
};

document.onkeypress = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('InvadersGame', 'JsKeyPress', evt.keyCode);

    //Prevent all but F5 and F12
    if (evt.keyCode !== 116 && evt.keyCode !== 123)
        evt.preventDefault();
};