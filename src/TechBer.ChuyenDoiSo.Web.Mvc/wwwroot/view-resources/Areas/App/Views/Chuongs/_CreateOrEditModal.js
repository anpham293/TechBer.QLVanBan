(function ($) {
    app.modals.CreateOrEditChuongModal = function () {

        var _chuongsService = abp.services.app.chuongs;

        var _modalManager;
        var _$chuongInformationForm = null;

		        var _ChuongcapQuanLyLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Chuongs/CapQuanLyLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Chuongs/_ChuongCapQuanLyLookupTableModal.js',
            modalClass: 'CapQuanLyLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chuongInformationForm = _modalManager.getModal().find('form[name=ChuongInformationsForm]');
            _$chuongInformationForm.validate();
        };

		          $('#OpenCapQuanLyLookupTableButton').click(function () {

            var chuong = _$chuongInformationForm.serializeFormToObject();

            _ChuongcapQuanLyLookupTableModal.open({ id: chuong.capQuanLyId, displayName: chuong.capQuanLyTen }, function (data) {
                _$chuongInformationForm.find('input[name=capQuanLyTen]').val(data.displayName); 
                _$chuongInformationForm.find('input[name=capQuanLyId]').val(data.id); 
            });
        });
		
		$('#ClearCapQuanLyTenButton').click(function () {
                _$chuongInformationForm.find('input[name=capQuanLyTen]').val(''); 
                _$chuongInformationForm.find('input[name=capQuanLyId]').val(''); 
        });
		


        this.save = function () {
            if (!_$chuongInformationForm.valid()) {
                return;
            }
            if ($('#Chuong_CapQuanLyId').prop('required') && $('#Chuong_CapQuanLyId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CapQuanLy')));
                return;
            }

            var chuong = _$chuongInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chuongsService.createOrEdit(
				chuong
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChuongModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);