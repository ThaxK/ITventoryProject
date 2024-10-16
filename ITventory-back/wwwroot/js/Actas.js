function mostrarPdfViewer() {
    $('#pdfviewer-container').show();
    $('#pdfViewerModal').modal('show');
}

function cerrarModal() {
    $('#pdfViewerModal').modal('hide');
}

function enviarImagenBase64(url, base64Image, Id, button, originalText) {
    $.ajax({
        url: url,
        type: "POST",
        data: { base64Image: base64Image, Id: Id },
        xhrFields: {
            responseType: 'blob'
        },
        success: function (response) {
            var employeeName = "@Model.Employee.Name";
            var employeeLastName = "@Model.Employee.LastName";

            var downloadName = url.includes("GenerarPDFFinalizacion") ? 'Acta Finalizacion' : 'Acta Inicial';

            var blob = new Blob([response], { type: 'application/pdf' });

            var pdfUrl = window.URL.createObjectURL(blob);

            mostrarPdfViewer(pdfUrl, `${downloadName}: ${employeeName}_${employeeLastName}.pdf`, blob);
        },
        error: function (error) {
            console.error(error);
            alert('Error al procesar la descarga');
        },
        complete: function () {
            $(button).removeClass('disabled').html(originalText);
        }
    });
}

function mostrarPdfViewer(pdfUrl, downloadFileName, blob) {

    var modalBody = $('#pdfViewerModal .modal-body');

    var embed = document.createElement('embed');
    embed.src = pdfUrl;
    embed.type = 'application/pdf';
    embed.style.width = '100%';
    embed.style.height = '500px';

    modalBody.empty().append(embed);

    $('#confirmarDescargaBtn').off('click').on('click', function () {

        var a = document.createElement('a');
        a.href = pdfUrl;
        a.download = downloadFileName;

        document.body.appendChild(a);
        a.click();

        window.URL.revokeObjectURL(pdfUrl);
        document.body.removeChild(a);

        window.close();
        cerrarModal();
        mostrarAlerta();
    });
    $('#pdfViewerModal').modal('show');
}
