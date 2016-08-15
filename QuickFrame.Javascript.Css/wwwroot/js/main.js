(function() {
  jQuery.extend({
    closeFancyboxAndRefreshParent: function() {
      parent.$.fancybox.close();
      return parent.$.reloadWindow();
    }
  });

  $(document).ready(function() {
    $('.datepicker').each(function() {
      return $(this).datepicker();
    });
    $('.remove-object').each(function() {
      return $(this).on('click', function(e) {
        e.preventDefault();
        return $.ajax({
          url: $(this).attr('href'),
          method: 'DELETE',
          success: function() {
            return window.location.reload();
          }
        });
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
    return $('.fancybox').each(function() {
      var _this, opts;
      _this = $(this);
      opts = {};
      opts.type = 'iframe';
      opts.autoSize = false;
      opts.width = $(this).attr('data-width') || '640px';
      opts.height = $(this).attr('data-height') || '480px';
      opts.closeBtn = $(this).is('[close-button]');
      opts.iframe = {};
      opts.iframe.scrolling = 'no';
      opts.afterClose = function() {
        return parent.window.location.reload();
      };
      return $(this).fancybox(opts);
    });
  });
}).call(this);