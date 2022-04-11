// from: https://stackoverflow.com/a/29641185
//if you have another AudioContext class use that one, as some browsers have a limit
//All arguments are optional:

//duration of the tone in milliseconds. Default is 500
//frequency of the tone in hertz. default is 440
//volume of the tone. Default is 1, off is 0.
//type of tone. Possible values are sine, square, sawtooth, triangle, and custom. Default is sine.
//callback to use on end of tone
window.audioContextBeep = (duration, frequency, volume, type, callback) => {
    //if you have another AudioContext class use that one, as some browsers have a limit
    var audioCtx = new (window.AudioContext || window.webkitAudioContext || window.audioContext);

    var oscillator = audioCtx.createOscillator();
    var gainNode = audioCtx.createGain();

    oscillator.connect(gainNode);
    gainNode.connect(audioCtx.destination);

    if (volume) {
        gainNode.gain.value = parseFloat(volume);
    }
    if (frequency) {
        oscillator.frequency.value = parseFloat(frequency);
    }
    if (type) {
        oscillator.type = type;
    }
    if (callback) {
        oscillator.onended = callback;
    }

    oscillator.start(audioCtx.currentTime);
    oscillator.stop(audioCtx.currentTime + ((parseFloat(duration) || 500) / 1000));
};