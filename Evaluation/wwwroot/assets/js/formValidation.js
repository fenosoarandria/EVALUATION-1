$(document).ready(function () {
    $('form').submit(function (event) {
        event.preventDefault();

        var form = $(this);
        var controller = form.data('controller');
        var action = form.data('action');

        $.ajax({
            url: '/' + controller + '/' + action,
            type: 'POST',
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    } else {
                        alert('Validation réussie');
                    }
                } else {
                    var errorMessages = '';
                    $.each(response.errors, function (key, value) {
                        errorMessages += '<p>' + value + '</p>';
                    });

                    $('#errorModal .modal-body').html(errorMessages);
                    $('#errorModal').modal('show');

                    // Ferme la modale après 5 secondes
                    setTimeout(function () {
                        $('#errorModal .modal-dialog').addClass('hide');
                        setTimeout(function () {
                            $('#errorModal').modal('hide');
                            $('#errorModal .modal-dialog').removeClass('hide');
                        }, 500); // Temps pour l'animation de fermeture
                    }, 3000);
                }
            }
        });
    });
});
