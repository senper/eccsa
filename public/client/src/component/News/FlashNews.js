import React from "react";
import { Link } from "react-router-dom";
import { getBusinessInformationList } from "../../services/businessInformation";
import "./FlashNews.css";

export default function FlashNews() {
  const [lst, setLst] = React.useState([]);
  React.useEffect(() => {
    getBusinessInformationList().then((data) => {
      setLst(data.data.$values);
      setKeyframeStr(getKeyframeStyle(data.data.$values));
    });
  }, []);
  const getKeyframeStyle = (inputLst) => {
    if (!inputLst || inputLst.length <= 0) return "";
    const percentRate = 100 / inputLst.length;
    const pxRate = 50;
    let keyframeStr = "@keyframes ticker { 0% {margin-top: -30px}";
    for (let i = 1; i < inputLst.length; i++) {
      keyframeStr += `${percentRate * i}% {margin-top: -${30 + pxRate * i}px}`;
    }
    keyframeStr += "}";
    return keyframeStr;
  };
  
  const [keyframeStr, setKeyframeStr] = React.useState(getKeyframeStyle(lst));

  return (
    <>
      <style>{keyframeStr}</style>
      {lst && lst.length > 0 ? (
        <main className="flash-news-main">
          <div className="news red full-width">
            <span>Breaking News</span>
            <ul className="scrollLeft">
              {lst.map((value) => (
                <li>
                  <Link 
                    to={{
                      pathname: "/membership",
                      state: {
                        eventKey: "membership",
                        newsId: value.id
                      },
                    }}
                  >
                    {" "}
                    {value.title}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        </main>
      ) : null}
    </>
  );
}
