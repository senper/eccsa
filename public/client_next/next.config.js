const nextTranslate = require("next-translate");
const withPWA = require("next-pwa");
const pwaConfig = withPWA({
  dest: "public",
  register: true,
  skipWaiting: true,
  disable: process.env.NODE_ENV === 'development'
});

/** @type {import('next').NextConfig} */
/* const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  poweredByHeader: false,
  images: {
    domains: ["ethiopianchamber.org.et"],
  },
}; */
const localhostFordDefaultSrc = process.env.NODE_ENV === 'development' ? "http://localhost:5000" : "";
const imageDomains = process.env.NODE_ENV === 'development' ? ["ethiopianchamber.org.et", "196.188.123.110", "http://localhost:5000"] : ["ethiopianchamber.org.et", "196.188.123.110"];
const ContentSecurityPolicy = `
  default-src 'self' https://app.ethiomobilemoney.et https://fonts.gstatic.com https://www.googletagmanager.com https://www.gstatic.com https://www.google.com https://www.google-analytics.com https://ethiopianchamber.et https://ethiopianchamber.org.et http://196.188.123.110 https://telebirr.ethiopianchamber.et ${localhostFordDefaultSrc};
  script-src 'self' https://app.ethiomobilemoney.et 'unsafe-eval' 'unsafe-inline' https://www.googletagmanager.com https://ethiopianchamber.et https://www.google.com https://www.gstatic.com/ ${localhostFordDefaultSrc};
  child-src 'self' https://app.ethiomobilemoney.et https://ethiopianchamber.et https://ethiopianchamber.com https://www.google.com https://www.youtube.com ${localhostFordDefaultSrc};
  style-src 'self' https://app.ethiomobilemoney.et https://ethiopianchamber.et https://ethiopianchamber.com 'unsafe-inline' ${localhostFordDefaultSrc};
  font-src 'self' https://app.ethiomobilemoney.et https://ethiopianchamber.et https://ethiopianchamber.com https://fonts.gstatic.com ${localhostFordDefaultSrc}; 
  img-src 'self' https: data:;
  frame-ancestors 'none'; 
  form-action 'self'  https://ethiopianchamber.et  https://ethiopianchamber.com  https://ethiopianchamber.org.et http://196.188.123.110;
`;
const securityHeaders = [
  {
    key: "X-DNS-Prefetch-Control",
    value: "on",
  },
  {
    key: "X-XSS-Protection",
    value: "1; mode=block",
  },
  {
    key: "X-Content-Type-Options",
    value: "nosniff",
  },
  {
    key: 'Referrer-Policy',
    value: 'strict-origin-when-cross-origin'
  },
  {
    key: 'Content-Security-Policy',
    value: ContentSecurityPolicy.replace(/\s{2,}/g, ' ').trim()
  }
];
const translateConfig = nextTranslate({
  webpack: (nextConfig, { isServer, webpack }) => {
    return nextConfig;
  },
  reactStrictMode: true,
  swcMinify: true,
  poweredByHeader: false,
  /* images: {
    domains: imageDomains,
  }, 
  async headers() {
    return [
      {
        // Apply these headers to all routes in your application.
        source: "/:path*",
        headers: securityHeaders,
      },
    ];
  },*/
});

module.exports = pwaConfig(translateConfig);
