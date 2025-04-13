// Version: 1.0.0

(function ($, window, document, undefined) {
    var modalTitle = '';
    var modalButtonOk = '';// 'بله';
    var modalButtonCancel = '';// 'خیر';
    var modalPreloaderTitle = '';// 'کمی صبر کنید...';
    var modalUploaderTitle = '';// 'درحال بارگذاری...';
    $.extend({
        prompt: function (value, title, callbackOk, callbackCancel) {
            if (arguments.length === 2) {
                callbackOk = arguments[1];
                title = arguments[0];
                value = '';
            }
            var m = modal({
                text: '<input class="modal-input" value="' + value + '"/>',
                title: typeof title === 'undefined' ? modalTitle : title,
                buttons: [
                    { text: modalButtonCancel, onClick: callbackCancel },
                    {
                        text: modalButtonOk, bold: true, onClick: function () {
                            var value = $('.modal-input').val();
                            callbackOk && callbackOk(value);
                        }
                    }
                ]
            });
            m.on('opened', function () {
                var $input = $('.modal-input');
                $input.focus();
                var input = $input.get(0);
                var value = $input.val();
                var valueLength = value ? value.length : 0;
                input.setSelectionRange && input.setSelectionRange(valueLength, valueLength);
            });
            return m;
        },
        modal: function (text, title, callbackOk) {
            if (typeof title === 'function') {
                callbackOk = arguments[1];
                title = undefined;
            }

            $('#iOSModal .modal-title').html(typeof title === 'undefined' ? modalTitle : title);
            $('#iOSModal .modal-text').html(text || '');

            // Add events on button
            $('#iOSModal .modal-footer span:first-child').unbind().click(callbackOk);

            $('#iOSModal').modal('show');
        },
        alert: function (text, title, callbackOk) {
            if (typeof title === 'function') {
                callbackOk = arguments[1];
                title = undefined;
            }

            $('#iOSAlert .modal-title').html(typeof title === 'undefined' ? modalTitle : title);
            $('#iOSAlert .modal-text').html(text || '');

            // Add events on button
            $('#iOSAlert .modal-footer span:first-child').unbind().click(callbackOk);

            $('#iOSAlert').modal('show');
        },
        confirm: function (text, title, callbackOk, callbackCancel) {
            if (typeof title === 'function') {
                callbackCancel = arguments[2];
                callbackOk = arguments[1];
                title = undefined;
            }

            $('#iOSConfirm .modal-title').html(typeof title === 'undefined' ? modalTitle : title);
            $('#iOSConfirm .modal-text').html(text || '');

            // Add events on buttons
            $('#iOSConfirm .modal-footer span:first-child').unbind().click(callbackOk);
            $('#iOSConfirm .modal-footer span:last-child').unbind().click(callbackCancel);

            $('#iOSConfirm').modal('show');
        },
        showPreloader: function (title) {
            $('#Preloader .modal-title').html(title || modalPreloaderTitle);
            $('#Preloader').modal('show');
        },
        hidePreloader: function () {
            setTimeout(function () {
                $('#Preloader').modal('hide');
            }, 600); 
        },
        showUploader: function (title) {
            $('#fileUpload .modal-title').html(title || modalUploaderTitle);
            $('#fileUpload .statustxt').html('0%');
            $('#fileUpload').modal('show');
        },
        hideUploader: function () {
            $('#fileUpload').modal('hide');
        },
        showScreenPreloader: function (title) {
            $('.loading').show();
            $('html, body').css({
                overflow: 'hidden',
                height: '100%'
            });
        },
        hideScreenPreloader: function () {
            $('#body').removeClass('d-none');
            $('.loading').hide();
            $('html, body').removeAttr('style');
        },
        notification: function (text, type, title, during) {
            if (!during) {
                during = 2000;
            }

            //switch (type) {
            //    case 'danger':
            //        $('#notification-title').html('خطا');
            //        break;
            //    case 'info':
            //        $('#notification-title').html('توجه');
            //        break;
            //    case 'success':
            //        $('#notification-title').html('پیام');
            //        break;
            //    case 'warning':
            //        $('#notification-title').html('اخطار');
            //        break;
            //    default:
            //        $('#notification-title').html('');
            //        type = 'primary';
            //}

            type = type || 'primary';
            $('#notification-title').html(title);

            $('.notification')
                .removeClass('alert-danger')
                .removeClass('alert-info')
                .removeClass('alert-success')
                .removeClass('alert-warning')
                .removeClass('alert-primary')
                .addClass('alert-' + type);

            $('#notification-message').html(text);

            $('.notification').removeClass('hide').addClass('show');

            setTimeout(function () {
                $('.notification').removeClass('show').addClass('hide');
            }, during);
        },
        actions: function (params) {
            var modal, groupSelector, buttonSelector;
            params = params || [];
            if (params.length > 0 && !$.isArray(params[0])) {
                params = [params];
            }
            var modalHTML;
            var buttonsHTML = '';
            for (var i = 0; i < params.length; i++) {
                for (var j = 0; j < params[i].length; j++) {
                    if (j === 0) buttonsHTML += '<div class="actions-modal-group">';
                    var button = params[i][j];
                    var buttonClass = button.label ? 'actions-modal-label' : 'actions-modal-button';
                    if (button.bold) buttonClass += ' actions-modal-button-bold';
                    if (button.color) buttonClass += ' color-' + button.color;
                    if (button.bg) buttonClass += ' bg-' + button.bg;
                    if (button.disabled) buttonClass += ' disabled';
                    buttonsHTML += '<div class="' + buttonClass + '">' + button.text + '</div>';
                    if (j === params[i].length - 1) buttonsHTML += '</div>';
                }
            }
            modalHTML = '<div class="actions-modal">' + buttonsHTML + '</div>';
            _modalTemplateTempDiv.innerHTML = modalHTML;
            modal = $(_modalTemplateTempDiv).children();
            $('body').append(modal[0]);
            groupSelector = '.actions-modal-group';
            buttonSelector = '.actions-modal-button';
            var groups = modal.find(groupSelector);
            groups.each(function (index, el) {
                var groupIndex = index;
                $(el).children().each(function (index, el) {
                    var buttonIndex = index;
                    var buttonParams = params[groupIndex][buttonIndex];
                    var clickTarget;
                    if ($(el).is(buttonSelector)) clickTarget = $(el);
                    if ($(el).find(buttonSelector).length > 0) clickTarget = $(el).find(buttonSelector);
                    if (clickTarget) {
                        clickTarget.on('click', function (e) {
                            if (buttonParams.close !== false) closeModal(modal);
                            if (buttonParams.onClick) buttonParams.onClick(modal, e);
                        });
                    }
                });
            });
            openModal(modal);
            return modal;
        }
    });
})(jQuery, window, document);