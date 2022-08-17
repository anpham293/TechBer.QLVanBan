(function ($) {
    app.modals.CreateOrEditLoaiDuAnModal = function () {

        var _loaiDuAnsService = abp.services.app.loaiDuAns;

        var _modalManager;
        var _$loaiDuAnInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$loaiDuAnInformationForm = _modalManager.getModal().find('form[name=LoaiDuAnInformationsForm]');
            _$loaiDuAnInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$loaiDuAnInformationForm.valid()) {
                return;
            }

            var loaiDuAn = _$loaiDuAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _loaiDuAnsService.createOrEdit(
				loaiDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLoaiDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);