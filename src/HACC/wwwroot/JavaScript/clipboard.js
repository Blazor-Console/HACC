window.clipboardFunctions = {
    setText: function (clipboardText) {
        navigator.clipboard.writeText(clipboardText).then(function () {
            return true;
        })
            .catch(function (error) {
                return false;
            });
    },
    getText: function () {
        navigator.clipboard.readText().then(function (text) {
            return text;
        })
            .catch(function (error) {
                return null;
            });
    }
}