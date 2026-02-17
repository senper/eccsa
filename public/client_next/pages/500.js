import Link from "next/link";

export default function Custom500() {
  return (
    <div id="notfound">
      <div className="notfound">
        <div className="notfound-404">
          <h1></h1>
        </div>
        <h2>We are sorry, Unfortunately, something has gone wrong!</h2>
        <p>
        We&apos;re unable to fulfill your request. Rest assured we have been notified and are looking into the issue. Please refresh your browser. If the error continues, please contact us.
        </p>
        <Link href="/">Back To Homepage</Link>
      </div>
    </div>
  );
}
