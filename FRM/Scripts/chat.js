$(function () {
    // Сгенерированный SignalR прокси для нашего хаба
    const chat = $.connection.chatHub;

    // Функция, которую сервер (Hub) будет вызывать для отображения нового сообщения
    chat.client.addNewMessageToPage = function (name, message, timestamp) {
        // HTML-кодирование для безопасности
        const encodedName = $('<div />').text(name).html();
        const encodedMsg = $('<div />').text(message).html();

        // Добавляем сообщение в список
        $('#messagesList').append('<div><strong>' + encodedName + ':</strong> ' + encodedMsg + ' <small class="text-muted">(' + timestamp + ')</small></div>');

        // Прокрутка вниз, чтобы видеть последнее сообщение
        const messagesDiv = document.getElementById("messagesList");
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
    };

    // Получаем элементы со страницы
    const $userName = $('#userName');
    const $messageText = $('#messageText');
    const $sendButton = $('#sendButton');

    // Устанавливаем фокус на поле ввода сообщения
    $messageText.focus();

    // Запускаем соединение с SignalR хабом
    $.connection.hub.start().done(function () {
        console.log("SignalR connection started.");

        // Обработчик нажатия на кнопку "Отправить"
        $sendButton.click(function () {
            sendMessage();
        });

        // Обработчик нажатия Enter в поле сообщения
        $messageText.keypress(function (e) {
            if (e.which == 13) { // 13 - код клавиши Enter
                e.preventDefault();
                sendMessage();
            }
        });

    }).fail(function (err) {
        console.error('SignalR connection failed: ' + err);
    });

    function sendMessage() {
        const name = $('#userName').val();
        const msg = $('#messageText').val();

        if (name.length > 0 && msg.length > 0) {
            console.log('Sending message to server:', name, msg);

            chat.server.sendMessage(name, msg);
            $('#messageText').val('').focus();
        }
    }
});