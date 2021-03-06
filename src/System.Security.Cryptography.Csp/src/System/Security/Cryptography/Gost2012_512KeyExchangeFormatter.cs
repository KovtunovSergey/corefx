﻿// Copyright (C) 2006-2012 Крипто-Про. Все права защищены.
//
// Этот файл содержит информацию, являющуюся
// собственностью компании Крипто-Про.
// 
// Любая часть этого файла не может быть скопирована,
// исправлена, переведена на другие языки,
// локализована или модифицирована любым способом,
// откомпилирована, передана по сети с или на
// любую компьютерную систему без предварительного
// заключения соглашения с компанией Крипто-Про.
// 

using CryptoPro.Sharpei.Properties;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CryptoPro.Sharpei
{
    /// <summary>
    /// Класс формирования данных для обмена симметричным ключом
    /// на основе <a href="http://www.ietf.org/rfc/rfc4490">ГОСТ Р 34.10 
    /// транспорта</a>.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>Класс позволяет отправителю сформировать зашифрованные 
    /// данные, которые получатель может расшифровать и использовать
    /// в качестве симметричного ключа для расшифрования сообщения.
    /// </para>
    /// <para>В отличии от аналогичных классов, порожденных от 
    /// <see cref="AsymmetricKeyExchangeFormatter"/>, данный класс
    /// нельзя использовать для получения произвольной общей информации,
    /// или произвольных симметричных ключей. Алгоритм предназначен
    /// <b>только</b> для форматирования данных на основе симметричного 
    /// ключа ГОСТ 28147.
    /// </para>
    /// <para>Для получения данных обмена ключами и извлечения 
    /// соответствующего симметричного ключа служит класс
    /// <see cref="Gost2012_512KeyExchangeDeformatter"/>.</para>
    /// </remarks>
    /// 
    /// <doc-sample path="Simple\Encrypt" name="KeyExchange2012_512">Пример работы с 
    /// форматтером и деформаттером обмена ключами.</doc-sample>
    /// <seealso cref="Gost2012_512KeyExchangeDeformatter"/>
    [ComVisible(true)]
    public class Gost2012_512KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
    {
        /// <summary>
        /// Создание объекта класса <see cref="Gost2012_512KeyExchangeFormatter"/>.
        /// </summary>
        public Gost2012_512KeyExchangeFormatter()
        {
        }

        /// <summary>
        /// Конструктор объекта класса <see cref="Gost2012_512KeyExchangeFormatter"/> 
        /// с заданным открытым ключом получателя.
        /// </summary>
        /// 
        /// <param name="key">Класс, содержащий ключ, для которого 
        /// будет производиться шифрование пердаваемой информации.</param>
        /// 
        /// <argnull name="key" />
        public Gost2012_512KeyExchangeFormatter(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            gostKey_ = (Gost3410_2012_512)key;
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="data">"Чистый" симметричный ключ 
        /// ГОСТ 28147.</param>
        /// 
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <remarks>
        /// <if notdefined="symimp"><para>В данной сборке функция всегда 
        /// возбуждает исключение <see cref="CryptographicException"/>.
        /// </para></if>
        /// <para>В зависимости от сборки функция может всегда возбуждать 
        /// исключение <see cref="CryptographicException"/>, так
        /// как использует "чистый" ключ. По возможности используйте 
        /// безопасную функцию 
        /// <see cref="CreateKeyExchange(SymmetricAlgorithm)"/></para>
        /// </remarks>
        public override byte[] CreateKeyExchange(byte[] data)
        {
            using (Gost28147CryptoServiceProvider alg =
                new Gost28147CryptoServiceProvider())
            {
                alg.Key = data;
                return CreateKeyExchangeData(alg);
            }
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="data">"Чистый" симметричный ключ 
        /// ГОСТ 28147.</param>
        /// <param name="symAlgType">Параметр не используется в
        /// этой версии.</param>
        /// 
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <remarks>
        /// <if notdefined="symimp"><para>В данной сборке функция всегда 
        /// возбуждает исключение <see cref="CryptographicException"/>.
        /// </para></if>
        /// <para>В зависимости от сборки функция может всегда возбуждать 
        /// исключение <see cref="CryptographicException"/>, так
        /// как использует "чистый" ключ. По возможности используйте 
        /// безопасную функцию 
        /// <see cref="CreateKeyExchange(SymmetricAlgorithm)"/></para>
        /// </remarks>
        public override byte[] CreateKeyExchange(byte[] data, Type symAlgType)
        {
            return CreateKeyExchange(data);
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="alg">Симметричный ключ ГОСТ 28147.</param>
        /// 
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <argnull name="alg" />
        public GostKeyTransport CreateKeyExchange(SymmetricAlgorithm alg)
        {
            return CreateKeyExchange(alg, CPUtils.DefaultKeyWrapMethods);
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="alg">Симметричный ключ ГОСТ 28147.</param>
        /// <param name="wrapMethod">Алгоритм экпорта</param>
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <argnull name="alg" />
        public GostKeyTransport CreateKeyExchange(SymmetricAlgorithm alg, GostKeyWrapMethod wrapMethod)
        {
            if (alg == null)
            {
                throw new ArgumentNullException("alg");
            }
            if (wrapMethod != GostKeyWrapMethod.CryptoProKeyWrap && wrapMethod != GostKeyWrapMethod.CryptoPro12KeyWrap)
            {
                throw new CryptographicException(
                    string.Format(Properties.Resources.Cryptography_UnsupportedWrapMethod, wrapMethod));
            }

            // Получаем параметры получателя.
            Gost3410Parameters senderParameters = gostKey_.ExportParameters(
                false);

            GostKeyTransportObject transport = new GostKeyTransportObject();

            // Создаем эфимерный ключ с параметрами получателя.
            using (Gost3410_2012_512EphemeralCryptoServiceProvider sender = new Gost3410_2012_512EphemeralCryptoServiceProvider(
                senderParameters))
            {
                // Создаем распределенный секрет.
                byte[] wrapped_data;
                using (GostSharedSecretAlgorithm agree = sender.CreateAgree(
                    senderParameters))
                {

                    // Зашифровываем симметричный ключ.
                    wrapped_data = agree.Wrap(alg,
                        wrapMethod);
                }

                GostWrappedKeyObject wrapped = new GostWrappedKeyObject();
                wrapped.SetByXmlWrappedKey(wrapped_data);

                transport.sessionEncryptedKey_ = wrapped;
                transport.transportParameters_ = new Gost3410CspObject();
                transport.transportParameters_.Parameters = sender.ExportParameters(false);
            }

            return transport.Transport;
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="alg">Симметричный ключ ГОСТ 28147.</param>
        /// 
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <argnull name="alg" />
        public byte[] CreateKeyExchangeData(SymmetricAlgorithm alg)
        {
            GostKeyTransport transport = CreateKeyExchange(alg, CPUtils.DefaultKeyWrapMethods);
            return transport.Encode();
        }

        /// <summary>
        /// Формирование данных обмена, на основе симметричного
        /// ключа шифрования сообщения ГОСТ 28147.
        /// </summary>
        /// 
        /// <param name="alg">Симметричный ключ ГОСТ 28147.</param>
        /// <param name="wrapMethod">Алгоритм экспорта</param>
        /// <returns>Зашифрованные данные для отправки стороне 
        /// получателю.</returns>
        /// 
        /// <argnull name="alg" />
        public byte[] CreateKeyExchangeData(SymmetricAlgorithm alg, GostKeyWrapMethod wrapMethod)
        {
            GostKeyTransport transport = CreateKeyExchange(alg, wrapMethod);
            return transport.Encode();
        }

        /// <summary>
        /// Устанавливает открытый ключ.
        /// </summary>
        /// 
        /// <param name="key">Алгоритм, содержащий открытый ключ 
        /// получателя.</param>
        /// 
        /// <remarks><para>
        /// Данный ключ необходимо установить до первого вызова фунций
        /// формирования обмена данных.</para></remarks>
        public override void SetKey(AsymmetricAlgorithm key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            gostKey_ = (Gost3410_2012_512)key;
        }

        /// <summary>
        /// Возвращает параметры обмена ключами.
        /// </summary>
        /// 
        /// <value>Всегда null.</value>
        /// 
        /// <remarks><para>Не используется.</para></remarks>
        public override string Parameters {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Ассиметричный ключ получателя.
        /// </summary>
        private Gost3410_2012_512 gostKey_;
    }

}
