document.getElementById('contact-form').addEventListener('submit', function (event) {
    event.preventDefault();

    const formData = new FormData(this);

    fetch('/Home/SubmitContactFormw', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            const responseMessage = document.getElementById('response-message');
            if (data.success) {
                responseMessage.textContent = 'Uğurla göndərildi!';
                responseMessage.className = 'response-message success';
            } else {
                responseMessage.textContent = 'Xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.';
                responseMessage.className = 'response-message error';
            }
        })
        .catch(error => {
            document.getElementById('response-message').textContent = 'Xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.';
            document.getElementById('response-message').className = 'response-message error';
        });
});