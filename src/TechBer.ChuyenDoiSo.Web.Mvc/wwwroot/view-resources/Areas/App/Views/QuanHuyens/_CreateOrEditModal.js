(function ($) {
    app.modals.CreateOrEditQuanHuyenModal = function () {

        var _quanHuyensService = abp.services.app.quanHuyens;

        var _modalManager;
        var _$quanHuyenInformationForm = null;

		        var _QuanHuyentinhThanhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuanHuyens/TinhThanhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuanHuyens/_QuanHuyenTinhThanhLookupTableModal.js',
            modalClass: 'TinhThanhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$quanHuyenInformationForm = _modalManager.getModal().find('form[name=QuanHuyenInformationsForm]');
            _$quanHuyenInformationForm.validate();
        };

		          $('#OpenTinhThanhLookupTableButton').click(function () {

            var quanHuyen = _$quanHuyenInformationForm.serializeFormToObject();

            _QuanHuyentinhThanhLookupTableModal.open({ id: quanHuyen.tinhThanhId, displayName: quanHuyen.tinhThanhName }, function (data) {
                _$quanHuyenInformationForm.find('input[name=tinhThanhName]').val(data.displayName); 
                _$quanHuyenInformationForm.find('input[name=tinhThanhId]').val(data.id); 
            });
        });
		
		$('#ClearTinhThanhNameButton').click(function () {
                _$quanHuyenInformationForm.find('input[name=tinhThanhName]').val(''); 
                _$quanHuyenInformationForm.find('input[name=tinhThanhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$quanHuyenInformationForm.valid()) {
                return;
            }
            if ($('#QuanHuyen_TinhThanhId').prop('required') && $('#QuanHuyen_TinhThanhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('TinhThanh')));
                return;
            }

            var quanHuyen = _$quanHuyenInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _quanHuyensService.createOrEdit(
				quanHuyen
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditQuanHuyenModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);