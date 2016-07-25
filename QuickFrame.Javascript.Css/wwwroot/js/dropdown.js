(function() {
  var loadDropdown;

  loadDropdown = function(selectControl, data) {
    var currentOptions, d, i, id, idField, initialBlank, initialText, initialVal, len, objData, opt, text, textField;
    if (data) {
      objData = data.$values || data;
    } else {
      return;
    }
    idField = selectControl.attr('data-id') || 'id';
    textField = selectControl.attr('data-text') || 'name';
    initialBlank = selectControl.attr('data-placeholder') ? true : false;
    initialText = selectControl.attr('data-initial-text');
    initialVal = selectControl.attr('data-initial-val');
    currentOptions = [];
    selectControl.find('option').each(function() {
      return currentOptions.push($(this).val());
    });
    selectControl.empty();
    if (initialBlank) {
      opt = $('<option>').text('');
      selectControl.append(opt);
    }
    for (i = 0, len = objData.length; i < len; i++) {
      d = objData[i];
      id = d[idField];
      text = d[textField];
      opt = $('<option>', {
        value: id
      }).text(text);
      if ((currentOptions && currentOptions.length > 0) && $.inArray(id.toString(), currentOptions) === -1) {
        opt.attr('selected', 'selected');
      }
      if (initialText && initialText === text) {
        opt.attr('selected', 'selected');
      }
      if (initialVal && initialVal.toString() === id.toString()) {
        opt.attr('selected', 'selected');
      }
      selectControl.append(opt);
    }
    selectControl.trigger('chosen:updated');
  };

  $(document).ready(function() {
    $('select[data-url]').each(function() {
      var _this, opts;
      _this = $(this);
      opts = {};
      if (_this.attr('data-placeholder')) {
        opts.allow_single_deselect = true;
      }
      opts.width = _this.attr('data-width') || '100%';
      _this.chosen(opts);
      return $.ajax({
        method: 'GET',
        url: _this.attr('data-url')
      }).done(function(html) {
        return loadDropdown(_this, html);
      });
    });
  });

}).call(this);
