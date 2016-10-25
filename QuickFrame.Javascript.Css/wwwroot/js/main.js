(function() {
  var search;

  search = function(searchBox) {
    if ($(searchBox).val()) {
      return window.location.href = $(searchBox).attr('search-url') + '?searchTerm=' + $(searchBox).val();
    } else {
      return window.location.href = $(searchBox).attr('search-url');
    }
  };

  $(document).ready(function() {
    $('.datepicker').each(function() {
      return $(this).datepicker();
    });
    $('.chosen').each(function() {
      return $this.chosen();
    });
    $('.remove-object').each(function() {
      return $(this).on('click', function(e) {
        e.preventDefault();
        if (confirm('Are you sure you wish to delete this object?')) {
          return $.ajax({
            url: $(this).attr('href'),
            method: 'DELETE',
            success: function() {
              return window.location.reload();
            }
          });
        }
      });
    });
    $('[email-src]').each(function() {
      return $(this).blur(function() {
        var dest;
        if ($(this).val().length > 0) {
          dest = '#' + $(this).attr('email-dest');
          return $(dest).val($(this).val().replace(' ', '.') + '@ffspaducah.com');
        }
      });
    });
    $('.fancybox').each(function() {
      var _this, opts, refresh;
      _this = $(this);
      refresh = $(this).is('[refresh]');
      opts = {};
      opts.type = 'iframe';
      opts.autoSize = false;
      opts.width = $(this).attr('data-width') || '640px';
      opts.height = $(this).attr('data-height') || '480px';
      opts.closeBtn = $(this).is('[close-button]');
      opts.iframe = {};
      if ($(this).is('[scrolling]')) {
        opts.iframe.scrolling = 'yes';
      } else {
        opts.iframe.scrolling = 'no';
      }
      opts.afterClose = function() {
        if (refresh) {
          return window.location.reload();
        }
      };
      return $(this).fancybox(opts);
    });
    $('#searchBox').change(function() {
      return search(this);
    });
    return $('#searchBox').keypress(function(event) {
      var keycode;
      keycode = event.keyCode ? event.keyCode : event.which;
      if (keycode === 13) {
        return search(this);
      }
    });
  });

}).call(this);
