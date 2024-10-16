$(document).ready(function () {
    var message = window.alertMessage;
    var icon = window.alertIcon;
    var backgroundColor;

    if (message) {
        Swal.fire({
            title: message,
            icon: icon,
            confirmButtonText: "OK",
            customClass: {
                popup: "animated bounceInDown",
                confirmButton: "btn-colorful"
            },
        }).then(function (result) {
            if (result.isConfirmed) {
                alertokClick();
            }
        });
    }
});



function mostrarAlerta() {
    Swal.fire({
        title: "Acta Descargada Exitosamente!",
        icon: "success",
        confirmButtonText: "OK",
        customClass: {
            popup: "animated bounceInDown",
            confirmButton: "btn-colorful"
        }
    }).then(function (result) {
        if (result.isConfirmed) {
            alertokClick();
        }
    });
}

