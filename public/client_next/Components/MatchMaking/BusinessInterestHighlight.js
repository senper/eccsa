import { useRouter } from "next/router";
import React from "react";
import { Button } from "react-bootstrap";
import { getSectorsList } from "../../api/businessInterest";

export default function BusinessInterestHighlight({ getLocal, getLocalByVal }) {
  var token = localStorage.getItem("access_token");
  const history = useRouter();
  const [sectors, setSectors] = React.useState([]);
  React.useEffect(() => {
    getSectorsList(true).then((data) => {
      if (data.data.sectors.$values) {
        setSectors(data.data.sectors.$values.filter((s) => !s.parentId));
      }
    });
  }, []);

  function addBusinessInterest() {
    return token
      ? history.push("/myServices?navTo=businessInterest")
      : history.push("/login");
  }
  return (
    <>
      <div className="bg">{getLocal("Business Interests In")}</div>
      <div style={{ maxHeight: "145px", overflowY: "scroll" }}>
        {sectors.map((s, index) => (
          <span key={index}>
            <Button
              style={{
                marginTop: "0",
                marginBottom: "10px",
                width: "100%",
              }}
              variant="outline-secondary"
              id={`button-business-interest-${index}`}
              onClick={() => {
                history.push(`/matchMaking?sectorId=${s.id}`);
              }}
            >
              {s.name}
            </Button>
            <br />
          </span>
        ))}
      </div>
      <Button
        type="submit"
        style={{
          float: "none",
          marginTop: "5px",
          marginBottom: "0",
          marginLeft: "40%",
        }}
        variant="outline-secondary"
        size="sm"
        id="button-add-business-interest"
        onClick={() => addBusinessInterest()}
      >
        {getLocal("Add Business Interest")}
      </Button>
    </>
  );
}
