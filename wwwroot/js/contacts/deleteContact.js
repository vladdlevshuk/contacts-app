$(document).ready(function () {
    $('.delete-btn').on('click', function () {
        const contactId = $(this).data('id');
        const contactName = $(this).data('name');

        $('#deleteContactName').text(contactName);

        $('#confirmDeleteBtn').on('click', function () {
            $.ajax({
                url: '/Contacts/Delete/' + contactId,
                type: 'DELETE',
                success: function () {
                    location.reload();
                },
                error: function () {
                    alert('Ошибка при удалении контакта.');
                }
            });
            $('#deleteConfirmModal').modal('hide');
        });
    });
});