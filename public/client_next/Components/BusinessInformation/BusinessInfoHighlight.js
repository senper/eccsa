import { useRouter } from "next/router";
import React from "react";
import { Button } from "react-bootstrap";
import { getBusinessInformationList } from "../../api/businessInformation";

export default function BusinessInfoHighlight({ getLocal, getLocalByVal }) {
  const history = useRouter();

  const [businessInfoList, setBusinessInfoList] = React.useState([]);
  React.useEffect(() => {
    getBusinessInformationList().then((data) => {
      setBusinessInfoList(data.data.$values);
    });
  }, []);

  return (
    <>
      <div className="bg">{getLocal("Business Information")}</div>
      <div style={{ maxHeight: "185px", overflowY: "scroll" }}>
        {businessInfoList.map((s, index) => (
          <span key={index}>
            <Button
              style={{
                marginTop: "0",
                marginBottom: "10px",
                width: "100%",
              }}
              variant="outline-secondary"
              id={`button-business-info-${index}`}
              onClick={() => {
                history.push(`/businessInformation?newsId=${s.id}`);
              }}
            >
              {getLocalByVal(s.title, s.titleLocal)}
            </Button>
            <br />
          </span>
        ))}
      </div>
    </>
  );
}
