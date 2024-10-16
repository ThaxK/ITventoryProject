 function toolbarClick(args) {
        var gridObj = document.getElementById("Grid").ej2_instances[0];
    if (args.item.id === 'Grid_excelexport') {
            var excelExportProperties = {
        header: {
        headerRows: 2,
    rows: [
    {cells: [{colSpan: 4, value: "NAVISAF S.A.S Nit: 9004717761", style: {fontColor: '#10437E', fontSize: 20, hAlign: 'Center', bold: true, } }] },
    {cells: [{colSpan: 4, value: "Reporte de Recursos", style: {fontColor: '#10437E', fontSize: 15, hAlign: 'Center', bold: true, } }] },
    ]
                },
    footer: {
        footerRows: 2,
    rows: [
    {cells: [{colSpan: 4, value: "Reporte generado desde: ITVENTORY", style: {hAlign: 'Center', bold: true } }] },
    {cells: [{colSpan: 4, value: "Gracias por usar nuestra plataforma", style: {hAlign: 'Center', bold: true } }] }
    ]
                },
            };
    gridObj.excelExport(excelExportProperties);
        }

    if (args.item.id === 'Grid_pdfexport') {
            var exportProperties = {
        header: {
        fromTop: 0,
    height: 120,
    contents: [
    {
        type: 'Text',
    value: "Navisaf S.A.S",
    position: {x: 0, y: 20 },
    style: {textBrushColor: '#10437E', fontSize: 25, alignment: 'Center' }
                        },
    {
        type: 'Text',
    value: "Nit: 9004717761",
    position: {x: 0, y: 50 },
    style: {textBrushColor: '#10437E', fontSize: 20, alignment: 'Center' }
                        },
    {
        type: 'Text',
        value: "Reporte de Recursos",
    position: {x: 0, y: 70 },
    style: {textBrushColor: '#10437E', fontSize: 20, alignment: 'Center' }
                        },
    {
        type: 'Line',
    style: {penColor: '#10437E', penSize: 2, dashStyle: 'Solid' },
    points: {x1: 0, y1: 100, x2: 685, y2: 100 }
                        },
    ]
                }
            }
    gridObj.pdfExport(exportProperties);
        }
    }