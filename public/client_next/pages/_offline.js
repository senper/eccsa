import Head from "next/head";

const Fallback = () => (
  <>
    <Head>
      <title>ECCSA Digital Service</title>
    </Head>
    <h4 className="text-center" style={{ fontWeight: "bold" }}>
      Unable to connect to ECCSA Server. Please check your network or Contact
      ECCSA Digital Service Support team
    </h4>
  </>
);

export default Fallback;
