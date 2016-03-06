namespace AlvinAshcraft.LinkBuilder
{
    using System;
    using System.Windows.Forms;

    public class CursorKeeper : IDisposable
    {
        private readonly Cursor _originalCursor;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorKeeper"/> class.
        /// </summary>
        /// <param name="newCursor">The new cursor.</param>
        public CursorKeeper(Cursor newCursor)
        {
            _originalCursor = Cursor.Current;
            Cursor.Current = newCursor;
        }

        #region " IDisposable Support "

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Cursor.Current = _originalCursor;
                }
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
