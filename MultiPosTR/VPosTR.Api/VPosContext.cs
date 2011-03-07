namespace VPosTR.Api
{
    public class VPosContext
    {
        private VPos _vpos;

        public VPosContext(VPos vpos)
        {
            this._vpos = vpos;
        }

        public PosResponse Sales(PosRequest posRequest)
        {
            return _vpos.Sales(posRequest);
        }

        public PosResponse Void(PosRequest posRequest)
        {
            return _vpos.Void(posRequest);
        }

        public PosResponse ReFund(PosRequest posRequest)
        {
            return _vpos.ReFund(posRequest);
        }
    }
}
