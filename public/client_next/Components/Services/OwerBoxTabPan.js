import Link from "next/link";
import { Tab } from "react-bootstrap";
import { CertificateOfOriginId } from "../../pages/api/servicetype";

export const TabPan = ({ so, s, getLocalByVal, getLocal }) => {
  var token = localStorage.getItem("access_token");
  const ToLocalStr = (value) => {
    return parseInt(value).toLocaleString();
  };

  const compare = (a, b) => {
    if (a.orderNo < b.orderNo) {
      return -1;
    }
    if (a.orderNo > b.orderNo) {
      return 1;
    }
    return 0;
  };

  const getRequestUrl = () => {
    const formUrl = `/myServices/serviceRequestForm?serviceTypeId=${s.id}`;
    if (token) {
      return formUrl;
    } else {
      return `/login?redirect=${formUrl}`;
    }
  };

  return (
    <Tab.Pane key={`${so.$id}-${s.$id}`} eventKey={s.id}>
      <pre>{s.description}</pre>
      {getLocal("Service Charge")} :- {getLocal("Members")}:{" "}
      {ToLocalStr(s.memberServiceCharge)} ; {getLocal("Other")}:{" "}
      {ToLocalStr(s.nonMemberServiceCharge)}
      <br /> {getLocal("Requirements")}
      <ol>
        {s.serviceRequirements.$values.sort(compare).map((r, index) => (
          <li key={index}>
            {getLocalByVal(r.nameEng, r.nameLocal)} -{" "}
            {getLocalByVal(r.descriptionEng, r.descriptionLocal)}
          </li>
        ))}
      </ol>
      {s.id == CertificateOfOriginId ? (
        <>
          <a className="btn btn-success" href="https://esw.et/esw-trd/">
            {getLocal("Request Service")}
          </a>
        </>
      ) : (
        <Link href={getRequestUrl()} passHref className="btn btn-success">
          {getLocal("Request Service")}
        </Link>
      )}
    </Tab.Pane>
  );
};
