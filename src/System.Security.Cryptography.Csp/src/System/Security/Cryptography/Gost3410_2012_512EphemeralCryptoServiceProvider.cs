using Internal.NativeCrypto;
using static Internal.NativeCrypto.CapiHelper;
using System.IO;

namespace System.Security.Cryptography
{
    /// <summary>
    /// �������� ������������ ����� ������ (SharedSecret) �� ������ 
    /// ��������� ���� � 34.10,
    /// ���������� ����� � ����������������.
    /// </summary>
    /// 
    /// <cspversions />
    public sealed class Gost3410_2012_512EphemeralCryptoServiceProvider : Gost3410_2012_512
    {
        /// <summary>
        /// HANDLE �����.
        /// </summary>
        private SafeKeyHandle _safeKeyHandle;
        /// <summary>
        /// HANDLE ����������.
        /// </summary>
        private SafeProvHandle _safeProvHandle;

        /// <summary>
        /// ��������� �������� (�� ���������) HANDLE key.
        /// </summary>
        /// 
        /// <unmanagedperm action="LinkDemand" />
        internal SafeKeyHandle SafeKeyHandle
        {
            get
            {
                return _safeKeyHandle;
            }
        }

        /// <summary>
        /// ��������� �������� (�� ���������) HANDLE key.
        /// </summary>
        /// 
        /// <unmanagedperm action="Demand" />
        public IntPtr KeyHandle
        {
            get
            {
                return SafeKeyHandle.DangerousGetHandle();
            }
        }

        /// <summary>
        /// ��������� �������� HANDLE ���������� ��� ��������� RefCount.
        /// </summary>
        /// 
        /// <unmanagedperm action="Demand" />
        public IntPtr ProviderHandle
        {
            get
            {
                return SafeProvHandle.DangerousGetHandle();
            }
        }

        /// <summary>
        /// ��������� �������� HANDLE ���������� ��� AddRef.
        /// </summary>
        /// 
        /// <unmanagedperm action="LinkDemand" />
        internal SafeProvHandle SafeProvHandle
        {
            get
            {
                return _safeProvHandle;
            }
        }

        /// <summary>
        /// ������������� ��������� � ��������� �����������. 
        /// </summary>
        /// <param name="basedOn">��������� ���������, �� ������ ��������
        /// ����� ������������ ��������� �������� ����. ������������
        /// OID ����������� � ��������� �����, ��������� ��������� �� 
        /// ������������.</param>
        public Gost3410_2012_512EphemeralCryptoServiceProvider(Gost3410Parameters basedOn)
        {
            _safeKeyHandle = SafeKeyHandle.InvalidHandle;
            _safeProvHandle = AcquireSafeProviderHandle();

            // ��������� ���������� ����� ��� ����������� �������� ������������.
            // ��������� ����� �� �����.
            CapiHelper.GenerateKey(_safeProvHandle,
                GostConstants.ALG_SID_DH_GR3410_12_512_EPHEM, CspProviderFlags.NoFlags,
                GostConstants.GOST3410_2012_512KEY_SIZE, basedOn.DigestParamSet,
                basedOn.PublicKeyParamSet, out _safeKeyHandle);
        }

        /// <summary>
        /// ������������� ��������� � ����������� ��������� 
        /// ������ ���������� CSP.
        /// </summary>
        public Gost3410_2012_512EphemeralCryptoServiceProvider()
        {
            _safeKeyHandle = SafeKeyHandle.InvalidHandle;
            CapiHelper.GenerateKey(_safeProvHandle,
                GostConstants.ALG_SID_DH_GR3410_12_512_EPHEM, (CspProviderFlags)0,
                GostConstants.GOST3410_2012_512KEY_SIZE, out _safeKeyHandle);
        }

        /// <summary>
        /// ������� ���������� ���������.
        /// </summary>
        /// 
        /// <param name="includePrivateParameters"><see langword="true"/>, 
        /// ��� �������� ���������� �����.</param>
        /// 
        /// <returns>��������� ���������.</returns>
        /// 
        /// <exception cref="CryptographicException">��� ��������
        /// ���������� �����.</exception>
        /// 
        /// <remarks>
        /// <if notdefined="userexp"><para>�� ������������ ������������ 
        /// � ������ ������ ��� �������� 
        /// ���������� ����� ������ ���������� ���������� 
        /// <see cref="CryptographicException"/>.</para></if>
        /// </remarks>
        public override Gost3410Parameters ExportParameters(
            bool includePrivateParameters)
        {
            if (includePrivateParameters)
            {
                throw new CryptographicException(SR.Argument_InvalidValue, "includePrivateParameters equal true ");
            }
            Gost3410CspObject obj1 = new Gost3410CspObject();
            CapiHelper.ExportPublicKey(_safeKeyHandle, obj1, CspAlgorithmType.Gost2012_512);
            return obj1.Parameters;
        }

        /// <summary>
        /// ������ ���������� ���������.
        /// </summary>
        /// 
        /// <param name="parameters">��������� ���������.</param>
        /// 
        /// <exception cref="CryptographicException">������.
        /// </exception>
        public override void ImportParameters(Gost3410Parameters parameters)
        {
            // ������ ��������� ����� - ��� �������� agree, 
            // ������������� ���������� �� ��������������.
            // ����� ����� ������������� ������ ������ private
            throw new NotSupportedException();
        }

        /// <summary>
        /// �������� ����� ������������.
        /// </summary>
        /// 
        /// <param name="alg">��������� ��������� �����.</param>
        /// 
        /// <returns>�������������� ������.</returns>
        /// 
        /// <intdoc>�� ��������� ����������� SharedSecret,
        /// �� ���������� �� ���������� ������ �������� ����.</intdoc>
        public override GostSharedSecretAlgorithm CreateAgree(
            Gost3410Parameters alg)
        {
            // ���������� ��� � ������ ��� ��������.
            Gost3410CspObject obj1 = new Gost3410CspObject(alg);

            return new GostSharedSecretCryptoServiceProvider(_safeKeyHandle,
                _safeProvHandle, obj1, CspAlgorithmType.Gost2012_512);
        }

        /// <summary>
        /// ������������ �������� ������� ����������� ������.
        /// </summary>
        /// 
        /// <param name="disposing"><see langword="true"/>, ���� �������� 
        /// ������ � ������ ��������, <see langword="false"/> - ������ 
        /// ������� ����� ���� ����������.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if ((_safeKeyHandle != null) && !_safeKeyHandle.IsClosed)
                _safeKeyHandle.Dispose();
            if ((_safeProvHandle != null) && !_safeProvHandle.IsClosed)
                _safeProvHandle.Dispose();

            base.Dispose(disposing);
        }

        private SafeProvHandle AcquireSafeProviderHandle()
        {
            SafeProvHandle safeProvHandleTemp;
            CapiHelper.AcquireCsp(new CspParameters(GostConstants.PROV_GOST_2012_512), out safeProvHandleTemp);
            return safeProvHandleTemp;
        }

        public override byte[] SignHash(byte[] hash)
        {
            throw new NotSupportedException();
        }

        public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm)
        {
            throw new NotSupportedException();
        }

        public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm)
        {
            throw new NotSupportedException();
        }

        protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
        {
            throw new NotSupportedException();
        }

        protected override byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
        {
            throw new NotSupportedException();
        }
    }
}
