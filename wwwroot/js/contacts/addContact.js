$(document).ready(function () {
    $('#addContactForm').submit(function (e) {
        e.preventDefault();

        const formData = $(this).serialize();

        $.post('/Contacts/Create', formData)
            .done(function () {
                location.reload();
            })
            .fail(function () {
                alert('Ошибка при добавлении контакта.');
            });
    });
});