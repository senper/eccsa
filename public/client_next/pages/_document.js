import { Html, Head, Main, NextScript } from "next/document";

export default function Document() {
  return (
    <Html lang="en">
      <Head>
        <link rel="stylesheet" href={"/styles/font-awesome-icons.css"} />
        <link rel="stylesheet" href={"/styles/font-awesome-icons2.css"} />
        <link rel="shortcut icon" href={"/images/logo.png"} />
        <link rel="manifest" href="/manifest.json" />

        <link rel="apple-touch-icon" href="/icons/apple-touch-icon.png" />
        <link
          rel="apple-touch-icon"
          sizes="152x152"
          href="/icons/apple-touch-icon-152x152.png"
        />
        <link
          rel="apple-touch-icon"
          sizes="180x180"
          href="/icons/apple-touch-icon-180x180.png"
        />
        <link
          rel="apple-touch-icon"
          sizes="167x167"
          href="/icons/apple-touch-icon-167x167.png"
        />
        <link
          rel="icon"
          type="image/png"
          sizes="32x32"
          href="/icons/icon-32x32.png"
        />
        <link
          rel="icon"
          type="image/png"
          sizes="16x16"
          href="/icons/icon-16x16.png"
        />
        <link rel="mask-icon" href="/icons/mask-icon.svg" color="#198754" />
      </Head>
      <body>
        <Main />
        <NextScript />
      </body>
    </Html>
  );
}
