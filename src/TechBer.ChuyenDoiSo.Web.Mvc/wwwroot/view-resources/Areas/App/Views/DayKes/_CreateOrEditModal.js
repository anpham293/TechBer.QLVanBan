(function ($) {
    app.modals.CreateOrEditDayKeModal = function () {

        var _dayKesService = abp.services.app.dayKes;

        var _modalManager;
        var _$dayKeInformationForm = null;

		        var _DayKephongKhoLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DayKes/PhongKhoLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DayKes/_DayKePhongKhoLookupTableModal.js',
            modalClass: 'PhongKhoLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$dayKeInformationForm = _modalManager.getModal().find('form[name=DayKeInformationsForm]');
            _$dayKeInformationForm.validate();
        };

		          $('#OpenPhongKhoLookupTableButton').click(function () {

            var dayKe = _$dayKeInformationForm.serializeFormToObject();

            _DayKephongKhoLookupTableModal.open({ id: dayKe.phongKhoId, displayName: dayKe.phongKhoMaSo }, function (data) {
                _$dayKeInformationForm.find('input[name=phongKhoMaSo]').val(data.displayName); 
                _$dayKeInformationForm.find('input[name=phongKhoId]').val(data.id); 
            });
        });
		
		$('#ClearPhongKhoMaSoButton').click(function () {
                _$dayKeInformationForm.find('input[name=phongKhoMaSo]').val(''); 
                _$dayKeInformationForm.find('input[name=phongKhoId]').val(''); 
        });
		


        this.save = function () {
            if (!_$dayKeInformationForm.valid()) {
                return;
            }
            if ($('#DayKe_PhongKhoId').prop('required') && $('#DayKe_PhongKhoId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('PhongKho')));
                return;
            }

            var dayKe = _$dayKeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _dayKesService.createOrEdit(
				dayKe
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDayKeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);