const token = document.querySelector('#FinnhubToken').value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; // get symbol from input hidden

// Connection opened. Subscribe to a symbol
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }));
});

// Listen (ready to receive) for message
socket.addEventListener('message', function (event) {
    try {
        var eventData = JSON.parse(event.data);

        // if error message is received from server
        if (eventData.type === "error") {
            $(".price").text(eventData.msg);
            return; // exit the function
        }

        // data received from server
        // Sample response:
        // {"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}

        if (eventData.type === 'trade' && eventData.data && eventData.data.length > 0) {
            // get the updated price
            var updatedPrice = eventData.data[0].p;

            // update the UI
            $(".price").text(updatedPrice.toFixed(2)); // price - big display
        }
    } catch (e) {
        console.error('Error parsing message data:', e);
    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    // disconnect from server
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }));
}

// when the page is being closed, unsubscribe from the websocket
window.onunload = function () {
    unsubscribe(stockSymbol);
}
