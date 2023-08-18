(function ($) {
    app.modals.CreateOrEditUserDuAnModal = function () {

        var _userDuAnsService = abp.services.app.userDuAns;

        var _modalManager;
        var _$userDuAnInformationForm = null;

		        var _UserDuAnuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserDuAns/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserDuAns/_UserDuAnUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _UserDuAnduAnLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/UserDuAns/DuAnLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/UserDuAns/_UserDuAnDuAnLookupTableModal.js',
            modalClass: 'DuAnLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userDuAnInformationForm = _modalManager.getModal().find('form[name=UserDuAnInformationsForm]');
            _$userDuAnInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var userDuAn = _$userDuAnInformationForm.serializeFormToObject();

            _UserDuAnuserLookupTableModal.open({ id: userDuAn.userId, displayName: userDuAn.userName }, function (data) {
                _$userDuAnInformationForm.find('input[name=userName]').val(data.displayName); 
                _$userDuAnInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$userDuAnInformationForm.find('input[name=userName]').val(''); 
                _$userDuAnInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenDuAnLookupTableButton').click(function () {

            var userDuAn = _$userDuAnInformationForm.serializeFormToObject();

            _UserDuAnduAnLookupTableModal.open({ id: userDuAn.duAnId, displayName: userDuAn.duAnName }, function (data) {
                _$userDuAnInformationForm.find('input[name=duAnName]').val(data.displayName); 
                _$userDuAnInformationForm.find('input[name=duAnId]').val(data.id); 
            });
        });
		
		$('#ClearDuAnNameButton').click(function () {
                _$userDuAnInformationForm.find('input[name=duAnName]').val(''); 
                _$userDuAnInformationForm.find('input[name=duAnId]').val(''); 
        });
		


        this.save = function () {
            if (!_$userDuAnInformationForm.valid()) {
                return;
            }
            if ($('#UserDuAn_UserId').prop('required') && $('#UserDuAn_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#UserDuAn_DuAnId').prop('required') && $('#UserDuAn_DuAnId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DuAn')));
                return;
            }

            var userDuAn = _$userDuAnInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _userDuAnsService.createOrEdit(
				userDuAn
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUserDuAnModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);