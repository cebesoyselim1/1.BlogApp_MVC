$(document).ready(function () {
    // Trumbowyg starts here
    $('#text-editor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontfamily'],
            ['fontsize']
        ],
        plugins: {
            colors: {
                foreColorList: [
                    'ff0000', '00ff00', '0000ff'
                ],
                backColorList: [
                    '000', '333', '555'
                ],
                displayAsList: false
            }
        }
    }); 
    // Trumbowgy ends here

    // Select2 starts here
    $('#categoryList').select2({
        placeholder: 'Please enter a category...',
        allowClear: true
    });
    // Select2 ends here

    // Datepicker starts here
    $(function () {
        $("#datepicker").datepicker({
            duration: 1000,
            showAnim: "drop",
            showOptions: { direction:"down" },
            minDate: -3,
            maxDate: 3
        });
    })
    // Datepicket ends here
});