var oForm = $('#demo-form');

function showLoading() {
    //$body.addClass("loading");
    $(".modal-loading").fadeIn("fast");
}
function hideLoading() {
    //$body.removeClass("loading");
    $(".modal-loading").fadeOut("slow");
}

function resetForm() {
    $('#demo-form').trigger("reset");
    $('.firstinput').focus();
    $('#demo-form .icheckbox_flat-green').removeClass('checked');
}
function serializeForm(datatable_id) {
    var table = $(datatable_id).DataTable();
    var params = table.$('input,select,textarea').serializeArray();
    // Iterate over all form elements
    $.each(params, function () {
        // If element doesn't exist in DOM
        if (!$.contains(document, $('#demo-form')[this.name])) {
            // Create a hidden element
            $('#demo-form').append(
                $('<input>')
                    .attr('type', 'hidden')
                    .attr('name', this.name)
                    .val(this.value)
            );
        }
    });
    //table.rows().every(function () {
    //    console.log($node.find('input[name==]').val());
    //    $node.find('input', 'select', 'textarea').each(function () {
    //        $('#demo-form').append(
    //            $('<input>')
    //                .attr('type', 'hidden')
    //                .attr('name', this.name)
    //                .val(this.value)
    //        );
    //    })
    //})
}

function get(url) {
    try {
        var deferred = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                deferred.resolve(res);
            },
            error: function (err) {
                setSetatusBarMessage(err.responseText, true);
            }
        })
        return deferred.promise();
    }
    catch (ex) {
        console.log(ex)
    }
}

function init_ToolbarSysInfo() {
    //$('form').find('input, select, textarea')
    //    .hover(function () {
    //        var $CurrTab = $('ul#myTab li.active');

    //        var color = $("#status-footer").css("background-color");
    //        if (color !== "rgb(255, 0, 0)" && color !== "rgb(0, 128, 0)") {
    //            $("#status-footer").css("color", "black");
    //            $('#status-footer').text("Source: TABLENAME: " + $CurrTab.find('a').attr('id') + " ; HEADERNAME: " + $(this).attr('name') + "");
    //        }
    //    })
    //    .mouseout(function () {
    //        var color = $("#status-footer").css("background-color");
    //        if (color !== "rgb(255, 0, 0)" && color !== "rgb(0, 128, 0)") {
    //            $('#status-footer').text("");
    //        }
    //    });
    $('form').on('mouseenter', 'input,select,textarea', function () {

        var color = $("#status-footer").css("background-color");
        if (color !== "rgb(255, 0, 0)" && color !== "rgb(0, 128, 0)") {
            $("#status-footer").css("color", "black");

            var $a = $(this).attr('name');
            if ($a !== undefined) {
                var $b = [];
                var $b = $a.split(".");
                var $d = $b.slice(-2);
                var $tableName = $d[0].replace(/[[\]/0-9]/g, '');;
                var $ColumnName = $d[1];
                $('#status-footer').text(`Source: TABLE NAME: ${$tableName} | COLUMN NAME: ${$ColumnName}`);
            }
        }
    }).on('mouseleave', 'input,select,textarea', function () {
        var color = $("#status-footer").css("background-color");
        if (color !== "rgb(255, 0, 0)" && color !== "rgb(0, 128, 0)") {
            $('#status-footer').text("");
        }
    });
}

function setSetatusBarMessage(Msg = null, isError = false) {
    if (isError) {
        var $el = $("#status-footer"),
            x = 5000,
            originalColor = $el.css("background");
            

        $el.text(Msg)
        $el.css("background", "red");//Red c9302c
        $el.css("color", "white");
        setTimeout(function () {
            $el.css("background", "white");
            $el.css("color", "black");
            $el.text("")
        }, x);
    }
    else {
        var $el = $("#status-footer"),
            x = 5000,
            originalColor = $el.css("background");

        $el.text(Msg)
        $el.css("background", "green");//green #26b99a
        $el.css("color", "white");
        setTimeout(function () {
            $el.css("background", "white");
            $el.css("color", "black");
            $el.text("")
        }, x);
    }
    
}


var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};



function init_Docs() {
    //$('body').click(function () {
    //    var $el = $("#status-footer"),
    //        x = 5000,
    //        originalColor = $el.css("background");
    //    $el.css("background", originalColor);
    //    $el.css("color", "black");
    //});

    $(".only-numeric").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode

        if (!(keyCode >= 48 && keyCode <= 57)) {
            $(".error").css("display", "inline");
            return false;
        } else {
            $(".error").css("display", "none");
        }
    });

   
    $('.no-space').on('keydown', function (e) {
        if (e.keyCode === 32) {
            return false;
        }
    });

    $('.no-space').on('keypress', function (e) {
        if (e.which == 32) {
            return false;
        }
    });

    $('.only-numeric-wdot').keyup(function () {
        var val = $(this).val();
        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2)
                val = val.replace(/\.+$/, "");
        }
        $(this).val(val);
    });

    $('.no-space').on('paste', function (e) {
        e.preventDefault();
        var withoutSpaces = e.originalEvent.clipboardData.getData('Text');
        withoutSpaces = withoutSpaces.replace(/\s+/g, '');
        $(this).val(withoutSpaces);
    });


    $('.no-space').on('drop', function (e) {
        e.preventDefault();
        return false;
    });


    function isNumberKey(value) {
        var regex = /^[+-]?\d*\.?\d{0,9}$/;
        if (regex.test(value)) {
            return true;
        } else {
            return false;
        }
    }


    $(".isNumberKey").on('keyup', function () {
        if (!isNumberKey(this.value)) {
            $(this).val(function (index, value) {
                return value.substr(0, value.length - 1);
            });
        }
    });

    $(".isNumberKey").on('keydown', function () {
        if (!isNumberKey(this.value)) {
            $(this).val(function (index, value) {
                return value.substr(0, value.length - 1);
            });
        }
    });

    $(".isNumberKey").on('paste', function (e) {
        e.preventDefault();
        return false;
    });

    $('.isNumberKey').on('drop', function (e) {
        e.preventDefault();
        return false;
    });

    //$('.validate-email').on('blur', function () {
    //    var _email = $(this).val();
    //    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    //    return re.test(_email);
    //})

}

function init_keyPress() {
    $(".only-numeric").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode

        if (!(keyCode >= 48 && keyCode <= 57)) {
            $(".error").css("display", "inline");
            return false;
        } else {
            $(".error").css("display", "none");
        }
    });
}

function readfile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image_preview').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}



function readfiles(input) {
    var parent = $(input).parent().parent();

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(parent).find('#image_preview').attr('src', e.target.result);
            $(parent).find('.HasProfilePhoto').val(true)
            console.log(true);
        }

        reader.readAsDataURL(input.files[0]);
    }
}


//function idleTimer() {
//    var t;
//    alert('session start!');
//    window.onmousemove = resetTimer; 
//    window.onmousedown = resetTimer; 
//    window.onclick = resetTimer;    
//    window.onscroll = resetTimer;   
//    window.onkeypress = resetTimer;  
  
//    function logout() {
//        alert('session time out!');
//        window.location.href = '/login';  
//    }

//    function resetTimer() {
//        clearTimeout(t);
//        t = setTimeout(logout, 900000);
//    }
//}

//function pwdexp() {
//    var t;
//    function getExpiredDate() {
//        $.ajax({
//            type: 'GET',
//            url: '/Shared/PasswordExpiry',
//            success: function (res) {                
//                if (res.isValid) {
//                    var dateExpired = res.result.dateExpired;
//                    var serverDate = res.result.serverDate;
//                }
//            },
//            error: function (err) {
//            }
//        });
//    }
   
//    window.onload = chkpwdexp;
//    window.onmousemove = chkpwdexp;
//    window.onmousedown = chkpwdexp;
//    window.onclick = chkpwdexp;
//    window.onscroll = chkpwdexp;
//    window.onkeypress = chkpwdexp;

//    function chkpwdexp() {
//        clearTimeout(t);
//        t = setTimeout(getExpiredDate, 1000);
//    }
//}


$(document).ready(function () {

    hideLoading();
    init_Docs();
    //idleTimer();
    //pwdexp();
});
