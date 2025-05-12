$(document).ready(function () {
    $('.edit-btn').click(function () {
        var id = $(this).data('id');
        var name = $(this).data('name');
        var mobile = $(this).data('mobile');
        var job = $(this).data('job');
        var birth = $(this).data('birth');

        console.log(id, name, mobile, birth);

        $('#editContactId').val(id);
        $('#editName').val(name);
        $('#editMobilePhone').val(mobile);
        $('#editJobTitle').val(job);
        $('#editBirthDate').val(birth);
    });

    $('#editContactForm').submit(function (e) {
        e.preventDefault();

        const formData = $(this).serialize();

        $.ajax({
            url: '/Contacts/Edit',
            type: 'POST',
            data: formData,
            success: function () {
                location.reload();
            },
            error: function () {
                alert('Ошибка при редактировании контакта.');
            }
        });
    });
});